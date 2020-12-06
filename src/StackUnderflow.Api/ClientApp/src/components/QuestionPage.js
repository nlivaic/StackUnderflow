import React from "react";
import QuestionData from "./QuestionData";
import AnswersListData from "./AnswersListData";
import ManageAnswer from "./ManageAnswer";
import CommentsList from "./CommentsList.js";

const QuestionPage = (props) => {
  const { questionId } = props.match.params;
  return (
    <>
      <div>
        <div>
          <div>Voting</div>
          <QuestionData questionId={questionId} action="ReadAndEdit" />
          <CommentsList parentType="question" parentIds={{ questionId }} />
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
