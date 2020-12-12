import React from "react";
import QuestionData from "./QuestionData";
import AnswersListData from "./AnswersListData";
import ManageAnswer from "./ManageAnswer";
import CommentsList from "./CommentsList.js";
import ManageComment from "./ManageComment.js";

const QuestionPage = (props) => {
  const { questionId } = props.match.params;
  return (
    <>
      <div>
        <div>
          <div>Voting</div>
          <QuestionData questionId={questionId} action="ReadAndEdit" />
          <CommentsList parentType="question" parentIds={{ questionId }} />
          <ManageComment
            parentType="question"
            parentIds={{ questionId }}
            action="New"
          />
          <span>
            !!!!!!!!!!!!!!!!!!!!!!!!!!!!!Answers!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
          </span>
          <AnswersListData questionId={questionId} />
          <ManageAnswer questionId={questionId} action="New" />
        </div>
      </div>
    </>
  );
};

export default QuestionPage;
