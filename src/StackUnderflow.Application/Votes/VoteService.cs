using System;
using System.Threading.Tasks;
using StackUnderflow.Application.Votes.Models;
using StackUnderflow.Common.Caching;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Enums;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Infrastructure.Caching;

namespace StackUnderflow.Application.Votes
{
    public class VoteService : IVoteService
    {
        private readonly IVoteRepository _voteRepository;
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<Answer> _answerRepository;
        private readonly IRepository<Comment> _commentRepository;
        private readonly ICache _cache;

        public VoteService(
            IVoteRepository voteRepository,
            IRepository<Question> questionRepository,
            IRepository<Answer> answerRepository,
            IRepository<Comment> commentRepository,
            ICache cache)
        {
            _voteRepository = voteRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _commentRepository = commentRepository;
            _cache = cache;
        }

        public async Task<int> GetVotesSumAsync(Guid targetId, VoteTargetEnum voteTarget) =>
            await _cache.GetOrCreateConcurrentAsync(
                GetCachingKey(new VoteCachingContext(targetId, voteTarget)),
                () => _voteRepository.GetVotesSumAsync(targetId),
                60);

        public async Task ChangeCachedVotesSumAfterVoteCast(Vote vote)
        {
            switch (vote.VoteType)
            {
                case VoteTypeEnum.Upvote:
                    await IncrementCachedVotesSum(vote);
                    break;
                case VoteTypeEnum.Downvote:
                    await DecrementCachedVotesSum(vote);
                    break;
            }
        }

        public async Task ChangeCachedVotesSumAfterVoteRevoked(Vote vote)
        {
            switch (vote.VoteType)
            {
                case VoteTypeEnum.Upvote:
                    await DecrementCachedVotesSum(vote);
                    break;
                case VoteTypeEnum.Downvote:
                    await IncrementCachedVotesSum(vote);
                    break;
            }
        }

        public async Task<IVoteable> GetVoteableFromRepositoryAsync(VoteTargetEnum voteTarget, Guid voteTargetId)
        {
            return voteTarget switch
            {
                VoteTargetEnum.Question => _ = await _questionRepository.GetByIdAsync(voteTargetId),
                VoteTargetEnum.Answer => _ = await _answerRepository.GetByIdAsync(voteTargetId),
                VoteTargetEnum.Comment => _ = await _commentRepository.GetByIdAsync(voteTargetId),
                _ => throw new ArgumentException($"Unknown type: {voteTarget.GetType()}.")
            };
        }

        public IVoteable GetVoteable(Vote vote)
        {
            if (vote.Question != null)
            {
                return vote.Question;
            }
            else if (vote.Answer != null)
            {
                return vote.Answer;
            }
            else if (vote.Comment != null)
            {
                return vote.Comment;
            }
            else
            {
                throw new BusinessException($"Vote '{vote.Id}' does not have any targets mapped.");
            }
        }

        private static string GetCachingKey(VoteCachingContext vote) =>
            vote.Target switch
            {
                VoteTargetEnum.Question => CachingKeys.VotesSumForQuestion + vote.TargetId,
                VoteTargetEnum.Answer => CachingKeys.VotesSumForAnswer + vote.TargetId,
                VoteTargetEnum.Comment => CachingKeys.VotesSumForComment + vote.TargetId,
                _ => throw new ArgumentException($"Unknown vote target.")
            };

        private async Task<int> IncrementCachedVotesSum(Vote vote) =>
            await _cache.IncrementAndGetConcurrentAsync(
                GetCachingKey(new VoteCachingContext(vote.TargetId, vote.Target.Target)),
                () => _voteRepository.GetVotesSumAsync(vote.TargetId),
                60);

        private async Task<int> DecrementCachedVotesSum(Vote vote) =>
            await _cache.DecrementAndGetConcurrentAsync(
                GetCachingKey(new VoteCachingContext(vote.TargetId, vote.Target.Target)),
                () => _voteRepository.GetVotesSumAsync(vote.TargetId),
                60);
    }
}
