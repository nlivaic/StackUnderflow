import React, { useEffect, useState } from "react";
import * as answersApi from "../api/answersApi.js";
import * as commentsApi from "../api/commentsApi.js";
import Answer from "./Answer.js";
import Comment from "./Comment.js";

const AnswersList = ({ questionId }) => {
  const [isLoaded, setIsLoaded] = useState(false);
  const [answersList, setAnswersList] = useState([]);
  useEffect(() => {
    const getAnswers = async () => {
      const answersData = await answersApi.getAnswers(questionId);
      const commentsData = await commentsApi.getComments("answer", {
        questionId,
        answerIds: answersData.map((answer) => answer.id),
      });
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
  return (
    <div>
      <div>
        {!isLoaded
          ? "Loading answers..."
          : answersList.map((answer) => {
              return (
                <div key={answer.id}>
                  <Answer key={answer.id} {...answer} />
                  {answer.comments.map((comment) => (
                    <Comment key={comment.id} {...comment} />
                  ))}
                </div>
              );
            })}
      </div>
      1
    </div>
  );
};

export default AnswersList;
