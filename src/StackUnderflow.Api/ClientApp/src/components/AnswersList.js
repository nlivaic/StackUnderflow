import React, { useEffect, useState } from "react";
import * as answersApi from "../api/answersApi.js";
import * as commentsApi from "../api/commentsApi.js";
import ManageAnswer from "./ManageAnswer.js";
import Comment from "./Comment.js";

const AnswersList = ({ questionId }) => {
  const [isLoaded, setIsLoaded] = useState(false);
  const [answersList, setAnswersList] = useState([]);
  useEffect(() => {
    const getAnswers = async () => {
      const answersData = await answersApi.getAnswers(questionId);
      let commentsData = [];
      if (answersData.length > 0) {
        commentsData = await commentsApi.getComments("answer", {
          questionId,
          answerIds: answersData.map((answer) => answer.id),
        });
      }
      setAnswersList(
        answersData.map((answer) => {
          return {
            ...answer,
            comments: commentsData.filter(
              (comment) => comment.answerId === answer.id
            ),
          };
        })
      );
      setIsLoaded(true);
    };
    getAnswers();
  }, [questionId]);

  const renderAnswers = (answersList) =>
    answersList.length === 0
      ? "No answers"
      : answersList.map((answer) => {
          return (
            <div key={answer.id}>
              <ManageAnswer
                key={answer.id}
                answer={answer}
                questionId={questionId}
              />
              {answer.comments.map((comment) => (
                <Comment key={comment.id} {...comment} />
              ))}
            </div>
          );
        });

  return (
    <div>
      <div>{!isLoaded ? "Loading answers..." : renderAnswers(answersList)}</div>
    </div>
  );
};

export default AnswersList;
