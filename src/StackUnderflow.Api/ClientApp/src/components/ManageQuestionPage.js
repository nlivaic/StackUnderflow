import React, { useState } from "react";
import { useHistory } from "react-router-dom";
import * as questionApi from "../api/questionApi.js";
import QuestionForm from "./QuestionForm.js";

const ManageQuestionPage = () => {
  const [question, setQuestion] = useState({
    title: "",
    body: "",
    tagIds: [],
  });
  const [isSaving, setIsSaving] = useState(false);
  const [errors, setErrors] = useState({});
  const history = useHistory();

  const onInputChange = (e) => {
    let value =
      e.target.id === "tagIds" ? e.target.value.split(" ") : e.target.value;
    const newQuestion = { ...question, [e.target.id]: value };
    setQuestion(newQuestion);
  };

  const onSubmit = (e) => {
    e.preventDefault();
    if (isFormValid()) {
      questionApi
        .askQuestion(question)
        .then((responseData) => {
          history.push(`/questions/${responseData.id}`);
        })
        .then((error) => {
          setIsSaving(false);
          // setErrors({ onSave: error.message });
        });
      setIsSaving(true);
    }
  };

  const isFormValid = () => {
    debugger;
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
        onSubmit={onSubmit}
        isSaving={isSaving}
        errors={errors}
      ></QuestionForm>
    </div>
  );
};

export default ManageQuestionPage;
