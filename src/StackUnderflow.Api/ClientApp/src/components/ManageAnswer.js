import React, { useState } from "react";
import Answer from "./Answer.js";
import AnswerEdit from "./AnswerEdit.js";
import * as answersApi from "../api/answersApi.js";
import { getErrorMessage } from "../utils/getErrorMessage.js";

const ManageAnswer = ({ answer, questionId, action = "ReadAndEdit" }) => {
  const [isSaving, setIsSaving] = useState(false);
  const [errors, setErrors] = useState({});
  const [isEditingOrNew, setIsEditingOrNew] = useState(
    action === "New" ? true : false
  );
  const [editedAnswer, setEditedAnswer] = useState(
    answer
      ? answer
      : {
          body: "",
        }
  );

  const onEditToggleHandle = (e) => {
    e.preventDefault();
    if (answer.isOwner) {
      setIsEditingOrNew(!isEditingOrNew);
    }
  };

  const onSaveNewHandle = async (e) => {
    e.preventDefault();
    setIsSaving(true);
    try {
      await answersApi.postAnswer(editedAnswer, questionId);
    } catch (error) {
      setErrors({ onSave: getErrorMessage(error) });
    }
    setIsSaving(false);
  };

  const onSaveEditHandle = async (e) => {
    e.preventDefault();
    setIsSaving(true);
    try {
      await answersApi.editAnswer(editedAnswer, questionId, editedAnswer.id);
      setIsEditingOrNew(false);
    } catch (error) {
      setErrors({ onSave: getErrorMessage(error) });
    }
    setIsSaving(false);
  };

  const onInputChange = ({ target }) => {
    setEditedAnswer({ ...editedAnswer, [target.id]: target.value });
  };
  return (
    <div>
      {isEditingOrNew ? (
        <AnswerEdit
          answer={editedAnswer}
          onSaveNewAnswer={onSaveNewHandle}
          onSaveEditAnswer={onSaveEditHandle}
          onCancel={onEditToggleHandle}
          action={action}
          isSaving={isSaving}
          onInputChange={onInputChange}
          errors={errors}
        />
      ) : (
        <Answer answer={editedAnswer} onStartEditing={onEditToggleHandle} />
      )}
    </div>
  );
};

export default ManageAnswer;
