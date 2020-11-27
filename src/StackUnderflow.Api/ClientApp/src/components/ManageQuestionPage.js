import React, { useState } from "react";
import { useHistory } from "react-router-dom";
import * as questionApi from "../api/questionApi.js";
import QuestionForm from "./QuestionForm.js";

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
          // setErrors({ onSave: error.message });
        });
    } else {
      questionApi
        .askQuestion(question)
        .then((responseData) => {
          history.push(`/questions/${responseData.id}`);
        })
        .catch((error) => {
          setIsSaving(false);
          // setErrors({ onSave: error.message });
        });
    }
    setIsSaving(true);
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
        buttonText={questionToEdit ? "Submit Edit" : "Ask Question"}
        isSaving={isSaving}
        errors={errors}
        question={question}
      ></QuestionForm>
    </div>
  );
};

export default ManageQuestionPage;
