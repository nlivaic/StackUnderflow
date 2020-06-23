using System;
using System.Collections.Generic;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Mvc;
using StackUnderflow.Api.Models;
using StackUnderflow.Core.Entities;

namespace StackUnderflow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<QuestionResponse>> Get()
        {
            var questions = Builder<Question>
                .CreateListOfSize(40)
                .TheFirst(10)
                    .With(q => q.OwnerId, new Guid("00000000-0000-0000-0000-000000000001"))
                    .With(q => q.Comments, Builder<Comment>.CreateListOfSize(4)
                        .TheFirst(1).With(c => c.OwnerId, new Guid("00000000-0000-0000-0000-000000000002"))
                        .TheNext(1).With(c => c.OwnerId, new Guid("00000000-0000-0000-0000-000000000003"))
                        .TheNext(1).With(c => c.OwnerId, new Guid("00000000-0000-0000-0000-000000000004"))
                        .TheNext(1).With(c => c.OwnerId, new Guid("00000000-0000-0000-0000-000000000001"))
                        .Build())
                .TheNext(10)
                    .With(q => q.OwnerId, new Guid("00000000-0000-0000-0000-000000000002"))
                    .With(q => q.Comments, Builder<Comment>.CreateListOfSize(8)
                        .TheFirst(2).With(c => c.OwnerId, new Guid("00000000-0000-0000-0000-000000000004"))
                        .TheNext(3).With(c => c.OwnerId, new Guid("00000000-0000-0000-0000-000000000001"))
                        .TheNext(1).With(c => c.OwnerId, new Guid("00000000-0000-0000-0000-000000000002"))
                        .TheNext(2).With(c => c.OwnerId, new Guid("00000000-0000-0000-0000-000000000003"))
                        .Build())
                .TheNext(10)
                    .With(q => q.OwnerId, new Guid("00000000-0000-0000-0000-000000000003"))
                    .With(q => q.Comments, Builder<Comment>.CreateListOfSize(8)
                        .TheFirst(2).With(c => c.OwnerId, new Guid("00000000-0000-0000-0000-000000000002"))
                        .TheNext(1).With(c => c.OwnerId, new Guid("00000000-0000-0000-0000-000000000003"))
                        .TheNext(2).With(c => c.OwnerId, new Guid("00000000-0000-0000-0000-000000000004"))
                        .TheNext(3).With(c => c.OwnerId, new Guid("00000000-0000-0000-0000-000000000001"))
                        .Build())
                .TheNext(10)
                    .With(q => q.OwnerId, new Guid("00000000-0000-0000-0000-000000000004"))
                    .With(q => q.Comments, Builder<Comment>.CreateListOfSize(10)
                        .TheFirst(3).With(c => c.OwnerId, new Guid("00000000-0000-0000-0000-000000000001"))
                        .TheNext(1).With(c => c.OwnerId, new Guid("00000000-0000-0000-0000-000000000004"))
                        .TheNext(4).With(c => c.OwnerId, new Guid("00000000-0000-0000-0000-000000000002"))
                        .TheNext(2).With(c => c.OwnerId, new Guid("00000000-0000-0000-0000-000000000001"))
                        .Build())
                .Build();
            throw new NotImplementedException("");
        }

        [HttpGet("{id}")]
        public ActionResult<QuestionResponse> Get(Guid id)
        {
            throw new NotImplementedException("");
        }

        [HttpPost]
        public IActionResult Post([FromBody] QuestionCreateRequest request)
        {
            throw new NotImplementedException("");
        }
    }
}
