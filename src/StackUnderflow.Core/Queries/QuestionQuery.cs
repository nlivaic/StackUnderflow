// using System;
// using System.Collections.Generic;

// namespace StackUnderflow.Core.Queries
// {
//     public class QuestionQuery
//     {
//         public QuestionQuery()
//         {

//         }

//         public async QuestionModel Execute(Guid questionId)
//         {

//         }
//     }

//     public class QuestionModel
//     {
//         public string OwnerId { get; set; }
//         public string OwnerName { get; set; }
//         public string Title { get; set; }
//         public string Body { get; set; }
//         public DateTime CreatedAt { get; set; }
//         public List<AnswerModel> Answers { get; set; } = new List<AnswerModel>();
//         public List<CommentModel> Comments { get; set; } = new List<CommentModel>();
//         public List<TagModel> Tags { get; set; } = new List<TagModel>();

//     }
//     public class AnswerModel
//     {
//         public string OwnerId { get; set; }
//         public string Body { get; set; }
//     }

//     public class CommentModel
//     {
//         public string OwnerId { get; set; }
//         public string Body { get; set; }
//     }

//     public class TagModel
//     {
//         public string OwnerId { get; set; }
//         public string Body { get; set; }
//     }
// }