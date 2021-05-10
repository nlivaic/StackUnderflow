using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Enums;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;
using StackUnderflow.Core.Models.Votes;
using StackUnderflow.Infrastructure.Caching;

namespace StackUnderflow.Core.Services
{
    public class VoteService : IVoteService
    {
        private readonly IVoteRepository _voteRepository;
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<Answer> _answerRepository;
        private readonly IRepository<Comment> _commentRepository;
        private readonly IUnitOfWork _uow;
        private readonly BaseLimits _limits;
        private readonly ICache _cache;
        private readonly IMapper _mapper;

        public VoteService(IVoteRepository voteRepository,
            IRepository<Question> questionRepository,
            IRepository<Answer> answerRepository,
            IRepository<Comment> commentRepository,
            IUnitOfWork uow,
            BaseLimits limits,
            ICache cache,
            IMapper mapper)
        {
            _voteRepository = voteRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _commentRepository = commentRepository;
            _uow = uow;
            _limits = limits;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<VoteGetModel> GetVoteAsync(Guid voteId)
        {
            var vote = await _voteRepository.GetByIdAsync(voteId);
            var voteModel = _mapper.Map<VoteGetModel>(vote);
            return voteModel;
        }

        public async Task<VoteGetModel> CastVoteAsync(VoteCreateModel voteModel)
        {
            var matchedVote = await _voteRepository
                .GetSingleAsync(v => v.UserId == voteModel.UserId
                    && (v.QuestionId == null || v.QuestionId == voteModel.TargetId)
                    && (v.AnswerId == null || v.AnswerId == voteModel.TargetId)
                    && (v.CommentId == null || v.CommentId == voteModel.TargetId));
            if (matchedVote != null)
            {
                throw new BusinessException($"User already voted on {voteModel.VoteTarget} on '{matchedVote.CreatedOn}'.");
            }
            IVoteable target = await GetVoteableFromRepositoryAsync(voteModel.VoteTarget, voteModel.TargetId);
            if (target == null)
            {
                throw new EntityNotFoundException(voteModel.VoteTarget.ToString(), voteModel.TargetId);
            }
            var vote = Vote.CreateVote(voteModel.UserId, target, voteModel.VoteType);
            target.ApplyVote(vote);
            await _uow.SaveAsync();
            await ChangeCachedVotesSumAfterVoteCast(vote);
            return new VoteGetModel
            {
                VoteId = vote.Id
            };
        }

        private async Task ChangeCachedVotesSumAfterVoteCast(Vote vote)
        {
            switch (vote.VoteType)
            {
                case Vote.VoteTypeEnum.Upvote:
                    await IncrementCachedVotesSum(vote);
                    break;
                case Vote.VoteTypeEnum.Downvote:
                    await DecrementCachedVotesSum(vote);
                    break;
            }
        }

        public async Task RevokeVoteAsync(VoteRevokeModel voteModel)
        {
            var vote = (await _voteRepository
                .GetVoteWithTargetAsync(voteModel.UserId, voteModel.VoteId))
                ?? throw new EntityNotFoundException("No vote to revoke or user not owner of target vote.", voteModel.VoteId);
            if (vote.CreatedOn.Add(_limits.VoteEditDeadline) < DateTime.UtcNow)
                throw new BusinessException($"Vote with id '{voteModel.VoteId}' cannot be edited since more than '{_limits.VoteEditDeadline.Minutes}' minutes passed.");
            var voteable = GetVoteable(vote);
            voteable.RevokeVote(vote, _limits);
            _voteRepository.Delete(vote);
            await _uow.SaveAsync();
            await ChangeCachedVotesSumAfterVoteRevoked(vote);
        }

        private async Task ChangeCachedVotesSumAfterVoteRevoked(Vote vote)
        {
            switch (vote.VoteType)
            {
                case Vote.VoteTypeEnum.Upvote:
                    await DecrementCachedVotesSum(vote);
                    break;
                case Vote.VoteTypeEnum.Downvote:
                    await IncrementCachedVotesSum(vote);
                    break;
            }
        }

        private async Task<int> IncrementCachedVotesSum(Vote vote)
        {
            var cachingKey = GetCachingKey(vote);
            return await _cache.IncrementAndGetConcurrentAsync(cachingKey, () => _voteRepository.GetVotesSum(vote.TargetId), 60);
        }

        private async Task<int> DecrementCachedVotesSum(Vote vote)
        {
            var cachingKey = GetCachingKey(vote);
            return await _cache.DecrementAndGetConcurrentAsync(cachingKey, () => _voteRepository.GetVotesSum(vote.TargetId), 60);
        }

        private string GetCachingKey(Vote vote) =>
            vote.Target.Target switch
            {
                VoteTargetEnum.Question => CachingKeys.VotesSumForQuestion + vote.TargetId,
                VoteTargetEnum.Answer => CachingKeys.VotesSumForAnswer + vote.TargetId,
                VoteTargetEnum.Comment => CachingKeys.VotesSumForComment + vote.TargetId,
                _ => throw new ArgumentException($"Unknown vote target on vote {vote.Id}.")
            };

        private async Task<IVoteable> GetVoteableFromRepositoryAsync(VoteTargetEnum voteTarget, Guid voteTargetId)
        {
            return voteTarget switch
            {
                VoteTargetEnum.Question => _ = await _questionRepository.GetByIdAsync(voteTargetId),
                VoteTargetEnum.Answer => _ = await _answerRepository.GetByIdAsync(voteTargetId),
                VoteTargetEnum.Comment => _ = await _commentRepository.GetByIdAsync(voteTargetId),
                _ => throw new ArgumentException()
            };
        }

        private IVoteable GetVoteable(Vote vote)
        {
            if (vote.Question != null)
                return vote.Question;
            else if (vote.Answer != null)
                return vote.Answer;
            else if (vote.Comment != null)
                return vote.Comment;
            else
                throw new BusinessException($"Vote '{vote.Id}' does not have any targets mapped.");
        }

        private async Task AddVoteableToRepositoryAsync(VoteTargetEnum voteTarget, IVoteable voteable)
        {
            var task = voteTarget switch
            {
                VoteTargetEnum.Question => _questionRepository.AddAsync(voteable as Question),
                VoteTargetEnum.Answer => _answerRepository.AddAsync(voteable as Answer),
                VoteTargetEnum.Comment => _commentRepository.AddAsync(voteable as Comment),
                _ => throw new ArgumentException()          // Had to introduce this to avoid the warning.
            };
            await task;
        }
    }
}
