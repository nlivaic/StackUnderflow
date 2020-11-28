import React, { useEffect, useState } from "react";
import Question from "./Question";
import * as questionApi from "../api/questionApi.js";
import CommentsList from "./CommentsList.js";
import ManageQuestionPage from "./ManageQuestionPage";
import { useHistory } from "react-router-dom";

const QuestionData = ({ questionId }) => {
  const [isLoaded, setIsLoaded] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [isDeleting, setIsDeleting] = useState(false);
  const [question, setQuestion] = useState({});
  const [errors, setErrors] = useState({});
  useEffect(() => {
    const getQuestion = async () => {
      var data = await questionApi.getQuestion(questionId);
      setQuestion(data);
      setIsLoaded(true);
    };
    getQuestion();
  }, [questionId]);
  const history = useHistory();

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

  const onDeleteHandle = () => {
    questionApi
      .deleteQuestion(questionId)
      .then((_) => history.push("/"))
      .catch((error) => {
        setIsDeleting(false);
        if ([404, 409].includes(error.response.status)) {
          setErrors({
            onDelete:
              "Question you wanted to delete is not found. Maybe it was deleted earlier?",
          });
        }
      });
    setIsDeleting(true);
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
        onDelete={onDeleteHandle}
        errors={errors}
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
            <CommentsList parentType="question" parentIds={{ questionId }} />
          </>
        )}
      </div>
    </div>
  );
};

export default QuestionData;
