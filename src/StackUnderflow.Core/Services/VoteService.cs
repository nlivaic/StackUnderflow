using System;
using System.Linq;
using System.Threading.Tasks;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Common.Interfaces;
using StackUnderflow.Core.Entities;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;
using static StackUnderflow.Core.Models.VoteCreateModel;

namespace StackUnderflow.Core.Services
{
    public class VoteService : IVoteService
    {
        private readonly IVoteRepository _voteRepository;
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<Answer> _answerRepository;
        private readonly IRepository<Comment> _commentRepository;
        private readonly IUnitOfWork _uow;
        private readonly ILimits _limits;

        public VoteService(IVoteRepository voteRepository,
            IRepository<Question> questionRepository,
            IRepository<Answer> answerRepository,
            IRepository<Comment> commentRepository,
            IUnitOfWork uow,
            ILimits limits)
        {
            _voteRepository = voteRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _commentRepository = commentRepository;
            _uow = uow;
            _limits = limits;
        }

        public async Task CastVote(VoteCreateModel voteModel)
        {
            var vote = (await _voteRepository
                .ListAllAsync(v => v.OwnerId == voteModel.OwnerId
                    && (v.QuestionId == null || v.QuestionId == voteModel.TargetId)
                    && (v.AnswerId == null || v.AnswerId == voteModel.TargetId)
                    && (v.CommentId == null || v.CommentId == voteModel.TargetId)))
                .SingleOrDefault();
            if (vote != null)
            {
                throw new BusinessException($"User already voted on {voteModel.VoteTarget.ToString()} on '{vote.CreatedAt}'.");
            }
            IVoteable target = await GetVoteableFromRepository(voteModel.VoteTarget, voteModel.TargetId);
            target.ApplyVote(vote);
            await AddVoteableToRepository(voteModel.VoteTarget, target);
            await _uow.SaveAsync();
        }

        public async Task RevokeVote(VoteRevokeModel voteModel)
        {
            var vote = (await _voteRepository
                .GetVote(voteModel.OwnerId, voteModel.VoteId))
                ?? throw new BusinessException("No vote to revoke or user not owner of target vote.");
            if (vote.CreatedAt.Add(_limits.VoteEditDeadline) < DateTime.UtcNow)
                throw new BusinessException($"Vote with id '{voteModel.VoteId}' cannot be edited since more than '{_limits.VoteEditDeadline.Minutes}' minutes passed.");
            var voteable = GetVoteable(vote);
            voteable.RevokeVote(vote);
            RemoveVoteableFromRepository(voteModel.VoteTarget, vote);
            await _uow.SaveAsync();
        }

        private async Task<IVoteable> GetVoteableFromRepository(VoteTargetEnum voteTarget, Guid voteTargetId)
        {
            IVoteable target = null;
            return voteTarget switch
            {
                VoteTargetEnum.Question => target = await _questionRepository.GetByIdAsync(voteTargetId),
                VoteTargetEnum.Answer => target = await _answerRepository.GetByIdAsync(voteTargetId),
                VoteTargetEnum.Comment => target = await _commentRepository.GetByIdAsync(voteTargetId),
                _ => throw new ArgumentException()          // Had to introduce this to avoid the warning.
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

        private async Task AddVoteableToRepository(VoteTargetEnum voteTarget, IVoteable voteable)
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

        private void RemoveVoteableFromRepository(VoteTargetEnum voteTarget, Vote vote)
        {
            switch (voteTarget)
            {
                case VoteTargetEnum.Question:
                    _questionRepository.Delete(vote.Question);
                    break;
                case VoteTargetEnum.Answer:
                    _answerRepository.Delete(vote.Answer);
                    break;
                case VoteTargetEnum.Comment:
                    _commentRepository.Delete(vote.Comment);
                    break;
            }
        }
    }
}
