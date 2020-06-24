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
