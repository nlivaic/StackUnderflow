import React, { useEffect, useState } from "react";
import Question from "./Question";
import * as questionApi from "../api/questionApi.js";
import CommentsList from "./CommentsList.js";
import ManageQuestionPage from "./ManageQuestionPage";

const QuestionData = ({ questionId }) => {
  const [isLoaded, setIsLoaded] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [question, setQuestion] = useState({});
  const [commentIdentifiers] = useState({ questionId });
  useEffect(() => {
    const getQuestion = async () => {
      var data = await questionApi.getQuestion(questionId);
      setQuestion(data);
      setIsLoaded(true);
    };
    getQuestion();
  }, [questionId]);

  const onEditHandle = (e) => {
    e.preventDefault();
    setIsEditing(true);
  };

  const onEditedHandle = (editedQuestion) => {
    if (editedQuestion) {
      setQuestion({ ...question, ...editedQuestion });
    }
    setIsEditing(false);
  };

  const isEditingOrReading = () =>
    isEditing ? (
      <ManageQuestionPage questionToEdit={question} onEdited={onEditedHandle} />
    ) : (
      <Question
        title={question.title}
        body={question.body}
        username={question.username}
        hasAcceptedAnswer={question.hasAcceptedAnswer}
        isOwner={question.isOwner}
        createdOn={question.createdOn}
        tags={question.tags}
        onEdit={onEditHandle}
      />
    );

  return (
    <div>
      <div>
        {!isLoaded ? (
          "Loading..."
        ) : (
          <>
            {isEditingOrReading()}
            <CommentsList
              parentType="question"
              parentIds={commentIdentifiers}
            />
          </>
        )}
      </div>
    </div>
  );
};

export default QuestionData;
