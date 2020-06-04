// using System;
// using StackUnderflow.Common.Query;
// using StackUnderflow.Core.Entities;

// namespace StackUnderflow.Core.Specifications
// {
//     public class QuestionSpecification : BaseSpecification<Question>
//     {
//         public QuestionSpecification(Guid questionId)
//             : base(q => q.Id == questionId)
//         { }

//         public QuestionSpecification IncludeComments()
//         {
//             AddIncludes(query => query.Include(q => q.Comments));
//             return this;
//         }

//         public QuestionSpecification IncludeAnswersAndComments()
//         {
//             AddIncludes(query =>
//                 query
//                     .Include(q => q.Answers)
//                     .ThenInclude(a => a.Comments)
//                     .Include(q => q.Comments));
//             return this;
//         }

//         public QuestionSpecification IncludeQuestionCommentsAndAnswers()
//         {
//             AddIncludes(query =>
//                 query
//                     .Include(q => q.Answers)
//                     .Include(q => q.Comments));
//             return this;
//         }

//         public QuestionSpecification IncludeAnswers()
//         {
//             AddInclude(q => q.Answers);
//             return this;
//         }
//     }
// }