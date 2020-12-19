import React from "react";
import * as actionTypes from "../utils/actionTypes.js";

const AnswerEdit = ({
  answer,
  action,
  onSaveNewAnswer,
  onSaveEditAnswer,
  onInputChange,
  isSaving,
  onCancel,
  errors,
}) => {
  const getSaveButton = () => {
    switch (action) {
      case actionTypes.New:
        return (
          <button onClick={onSaveNewAnswer} disabled={isSaving}>
            {isSaving ? "Posting..." : "Post Your Answer"}
          </button>
        );
      case actionTypes.ReadAndEdit:
        return (
          <button onClick={onSaveEditAnswer} disabled={isSaving}>
            {isSaving ? "Saving..." : "Save Edits"}
          </button>
        );
      default:
        throw new Error("Unknown action.");
    }
  };
  const getCancelButton = () => {
    return action === actionTypes.ReadAndEdit ? (
      <button onClick={onCancel}>Cancel</button>
    ) : (
      ""
    );
  };
  return (
    <div style={{ borderStyle: "solid", borderColor: "green" }}>
      {errors.onSave ? (
        <span style={{ color: "red" }}>{errors.onSave}</span>
      ) : (
        ""
      )}
      <hr />
      <input
        type="text"
        id="body"
        placeholder="Enter answer here..."
        value={answer.body}
        onChange={onInputChange}
      />
      <br />

      {errors.body ? <span style={{ color: "red" }}>* {errors.body}</span> : ""}
      <br />
      <br />
      {getSaveButton()}
      {getCancelButton()}
    </div>
  );
};

export default AnswerEdit;
