import React from "react";
import * as actionTypes from "../utils/actionTypes.js";

const QuestionEdit = ({
  question,
  action,
  onSaveNewQuestion,
  onSaveEditQuestion,
  onInputChange,
  isSaving,
  onCancel,
  errors,
}) => {
  const getSaveButton = () => {
    switch (action) {
      case actionTypes.New:
        return (
          <button onClick={onSaveNewQuestion}>
            {isSaving ? "Posting..." : "Ask Question"}
          </button>
        );
      case actionTypes.ReadAndEdit:
        return (
          <button onClick={onSaveEditQuestion}>
            {isSaving ? "Saving..." : "Save Edits"}
          </button>
        );
      default:
        throw new Error("Unknown action.");
    }
  };
  const getCancelButton = () => {
    return <button onClick={onCancel}>Cancel</button>;
  };
  return (
    <div style={{ borderStyle: "solid", borderColor: "green" }}>
      <hr />
      {errors.apiError ? (
        <span style={{ color: "red" }}>{errors.apiError}</span>
      ) : (
        ""
      )}
      <br />
      <input
        type="text"
        onChange={onInputChange}
        id="title"
        placeholder="Title..."
        value={question.title}
      />
      {errors.title ? (
        <span style={{ color: "red" }}>* {errors.title}</span>
      ) : (
        ""
      )}
      <br />
      <input
        type="text"
        onChange={onInputChange}
        id="body"
        placeholder="Body..."
        value={question.body}
      />
      {errors.body ? <span style={{ color: "red" }}>* {errors.body}</span> : ""}
      <br />
      <input
        type="text"
        onChange={onInputChange}
        id="tags"
        placeholder="Tags..."
        value={question.tags.map((t) => t.id).join(" ")}
      />
      {errors.tags ? <span style={{ color: "red" }}>* {errors.tags}</span> : ""}
      <br />
      {getSaveButton()}
      {getCancelButton()}
    </div>
  );
};

export default QuestionEdit;
