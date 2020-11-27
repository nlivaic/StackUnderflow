import React, { useState } from "react";
import { useHistory } from "react-router-dom";
import * as questionApi from "../api/questionApi.js";
import QuestionForm from "./QuestionForm.js";
import { getErrorMessage } from "../utils/getErrorMessage.js";

const ManageQuestionPage = ({ questionToEdit, onEdited }) => {
  const [question, setQuestion] = useState(
    questionToEdit
      ? {
          title: questionToEdit.title,
          body: questionToEdit.body,
          tagIds: questionToEdit.tags.map((t) => t.id),
        }
      : {
          title: "",
          body: "",
          tagIds: [],
        }
  );
  const [isSaving, setIsSaving] = useState(false);
  const [isEditing] = useState(!!questionToEdit);
  const [errors, setErrors] = useState({});
  const history = useHistory();

  const onInputChange = (e) => {
    let value =
      e.target.id === "tagIds" ? e.target.value.split(" ") : e.target.value;
    const newQuestion = { ...question, [e.target.id]: value };
    setQuestion(newQuestion);
  };

  const onSubmitHandle = (e) => {
    e.preventDefault();
    if (!isFormValid()) {
      return;
    }
    if (questionToEdit) {
      questionApi
        .editQuestion(questionToEdit.id, question)
        .then((_) => onEdited(question))
        .catch((error) => {
          setIsSaving(false);
          setErrors({ onSave: getErrorMessage(error) });
        });
    } else {
      questionApi
        .askQuestion(question)
        .then((responseData) => {
          history.push(`/questions/${responseData.id}`);
        })
        .catch((error) => {
          setIsSaving(false);
          setErrors({ onSave: getErrorMessage(error) });
        });
    }
    setIsSaving(true);
  };

  const onCancelEditHandle = (e) => {
    e.preventDefault();
    onEdited();
  };

  const isFormValid = () => {
    const error = {};
    if (question.title.trim().length === 0) {
      error.title = "Question title not provided.";
    }
    if (question.body.trim().length < 100) {
      error.body = "Question body must be at least 100 characters.";
    }
    if (question.tagIds.length === 0) {
      error.tagIds = "At least one tag must be selected.";
    }
    setErrors(error);
    return Object.keys(error).length === 0;
  };

  return (
    <div>
      <QuestionForm
        onInputChange={onInputChange}
        onSubmit={onSubmitHandle}
        isSaving={isSaving}
        isEditing={isEditing}
        errors={errors}
        question={question}
        onCancelEdit={onCancelEditHandle}
      ></QuestionForm>
    </div>
  );
};

export default ManageQuestionPage;
