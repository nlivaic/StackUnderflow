import React from "react";

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
      case "New":
        return (
          <button onClick={onSaveNewAnswer}>
            {isSaving ? "Posting..." : "Post Your Answer"}
          </button>
        );
      case "ReadAndEdit":
        return (
          <button onClick={onSaveEditAnswer}>
            {isSaving ? "Saving..." : "Save Edits"}
          </button>
        );
      default:
        throw new Error("Unknown action.");
    }
  };
  const getCancelButton = () => {
    return action === "ReadAndEdit" ? (
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
