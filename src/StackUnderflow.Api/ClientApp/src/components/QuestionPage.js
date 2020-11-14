import React from "react";
import QuestionForm from "./QuestionForm";

const QuestionPage = (props) => {
  return (
    <>
      <h3>Question Page for {props.match.params.questionId}</h3>
      <div>
        <div>
          <QuestionForm id={props.match.params.questionId} />
        </div>
      </div>
    </>
  );
};

export default QuestionPage;
