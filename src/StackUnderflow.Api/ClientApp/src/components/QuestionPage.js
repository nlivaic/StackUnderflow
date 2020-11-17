import React from "react";
import QuestionForm from "./QuestionForm";
import AnswersList from "./AnswersList";

const QuestionPage = (props) => {
  const { questionId } = props.match.params;
  return (
    <>
      <div>
        <div>
          <div>Voting</div>
          <QuestionForm questionId={questionId} />
          <span>
            !!!!!!!!!!!!!!!!!!!!!!!!!!!!!Answers!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
          </span>
          <AnswersList questionId={questionId} />
        </div>
      </div>
    </>
  );
};

export default QuestionPage;
