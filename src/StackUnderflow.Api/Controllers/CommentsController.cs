using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StackUnderflow.Api.Models;
using StackUnderflow.Common.Exceptions;
using StackUnderflow.Core.Interfaces;
using StackUnderflow.Core.Models;

namespace StackUnderflow.Api.Controllers
{
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public CommentsController(
            ICommentRepository commentRepository,
            ICommentService commentService,
            IMapper mapper)
        {
            _commentRepository = commentRepository;
            _commentService = commentService;
            _mapper = mapper;
        }

        [HttpGet("api/questions/{questionId}/[controller]")]
        public async Task<ActionResult<IEnumerable<CommentGetViewModel>>> GetCommentForQuestion([FromRoute] Guid questionId)
        {
            var comment = await _commentRepository.GetCommentsForQuestion(questionId);
            IEnumerable<CommentGetViewModel> result = _mapper.Map<List<CommentGetViewModel>>(comment);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("api/questions/{questionId}/[controller]/{commentId}", Name = "GetForQuestion")]
        public async Task<ActionResult<CommentGetViewModel>> GetCommentsForQuestion([FromRoute] Guid questionId, [FromRoute] Guid commentId)
        {
            var comment = await _commentRepository.GetCommentModel(questionId, commentId);
            if (comment == null)
            {
                return NotFound();
            }
            CommentGetViewModel result = _mapper.Map<CommentGetViewModel>(comment);
            return Ok(result);
        }

        [HttpPost("/api/questions/{questionId}/[controller]")]
        public async Task<ActionResult<CommentGetViewModel>> PostOnQuestion([FromRoute] Guid questionId, [FromBody] CommentOnQuestionCreateRequest request)
        {
            var comment = _mapper.Map<CommentOnQuestionCreateModel>(request);
            comment.QuestionId = questionId;
            comment.UserId = new Guid("fa11acfe-8234-4fa3-9733-19abe08f74e8");       // @nl: from logged in user.
            CommentGetModel commentModel = null;
            try
            {
                commentModel = await _commentService.CommentOnQuestionAsync(comment);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            var result = _mapper.Map<CommentGetViewModel>(commentModel);
            return CreatedAtRoute("GetForQuestion", new { questionId = questionId, commentId = commentModel.Id }, result);
        }

        [HttpPut("/api/questions/{questionId}/[controller]/{commentId}")]
        public async Task<ActionResult> PutOnQuestion([FromRoute] Guid questionId, [FromRoute] Guid commentId, [FromBody] UpdateCommentOnQuestionRequest request)
        {
            var comment = _mapper.Map<CommentEditModel>(request);
            comment.ParentQuestionId = questionId;
            comment.UserId = new Guid("fa11acfe-8234-4fa3-9733-19abe08f74e8");       // @nl: from logged in user.
            comment.CommentId = commentId;
            try
            {
                await _commentService.EditAsync(comment);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
