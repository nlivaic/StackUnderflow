import React, { useEffect, useState } from "react";
import Question from "./Question";
import * as questionApi from "../api/questionApi.js";
import CommentsList from "./CommentsList.js";

const QuestionData = ({ questionId }) => {
  const [isLoaded, setIsLoaded] = useState(false);
  const [question, setQuestion] = useState({});
  useEffect(() => {
    const getQuestion = async () => {
      var data = await questionApi.getQuestion(questionId);
      setQuestion(data);
      setIsLoaded(true);
    };
    getQuestion();
  }, [questionId]);
  return (
    <div>
      <div>
        {!isLoaded ? (
          "Loading..."
        ) : (
          <>
            <Question
              title={question.title}
              body={question.body}
              username={question.username}
              hasAcceptedAnswer={question.hasAcceptedAnswer}
              createdOn={question.createdOn}
              tags={question.tags}
            />
            <CommentsList parentType="question" parentIds={{ questionId }} />
          </>
        )}
      </div>
    </div>
  );
};

export default QuestionData;
