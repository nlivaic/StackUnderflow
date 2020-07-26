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
        public async Task<ActionResult<IEnumerable<CommentGetViewModel>>> Get([FromRoute] Guid questionId)
        {
            IEnumerable<CommentGetViewModel> result = _mapper.Map<List<CommentGetViewModel>>(
                await _commentRepository.GetCommentsForQuestion(questionId));
            return Ok(result);
        }

        [HttpPost("/api/questions/{questionId}/[controller]")]
        public async Task<ActionResult<CommentGetViewModel>> Post([FromRoute] Guid questionId, [FromBody] CommentOnQuestionCreateRequest request)
        {
            var comment = _mapper.Map<CommentOnQuestionCreateModel>(request);
            comment.QuestionId = questionId;
            comment.UserId = new Guid("fa11acfe-8234-4fa3-9733-19abe08f74e8");       // @nl: from logged in user.
            var commentModel = await _commentService.CommentOnQuestionAsync(comment);
            var result = _mapper.Map<CommentGetViewModel>(commentModel);
            return Ok();
        }
    }
}
