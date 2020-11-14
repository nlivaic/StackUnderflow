import React, { useEffect, useState } from "react";
import Question from "./Question";
import * as questionApi from "../api/questionApi.js";
import CommentsList from "./CommentsList.js";

const QuestionForm = ({ id }) => {
  const [isLoaded, setIsLoaded] = useState(false);
  const [question, setQuestion] = useState({});
  useEffect(() => {
    const getQuestion = async () => {
      var data = await questionApi.getQuestion(id);
      setQuestion(data);
      setIsLoaded(true);
    };
    getQuestion();
  }, []);
  return (
    <div>
      <div>Voting</div>
      <div>
        {!isLoaded ? (
          "Loading..."
        ) : (
          <Question
            title={question.title}
            body={question.body}
            username={question.username}
            hasAcceptedAnswer={question.hasAcceptedAnswer}
            createdOn={question.createdOn}
            tags={question.tags}
          />
        )}
      </div>
      <CommentsList parentType="questions" parentId={id} />
    </div>
  );
};

export default QuestionForm;
