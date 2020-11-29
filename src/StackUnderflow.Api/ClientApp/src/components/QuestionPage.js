import React from "react";
import QuestionData from "./QuestionData";
import AnswersList from "./AnswersList";
import ManageAnswer from "./ManageAnswer";

const QuestionPage = (props) => {
  const { questionId } = props.match.params;
  return (
    <>
      <div>
        <div>
          <div>Voting</div>
          <QuestionData questionId={questionId} />
          <span>
            !!!!!!!!!!!!!!!!!!!!!!!!!!!!!Answers!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
          </span>
          <AnswersList questionId={questionId} />
          <ManageAnswer questionId={questionId} action="New" />
        </div>
      </div>
    </>
  );
};

export default QuestionPage;
