import React from "react";

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
      case "New":
        return (
          <button onClick={onSaveNewQuestion}>
            {isSaving ? "Posting..." : "Ask Question"}
          </button>
        );
      case "ReadAndEdit":
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
    return action === "ReadAndEdit" ? (
      <button onClick={onCancel}>Cancel</button>
    ) : (
      ""
    );
  };
  return (
    <div style={{ borderStyle: "solid", borderColor: "green" }}>
      <hr />
      {errors.onSave ? (
        <span style={{ color: "red" }}>{errors.onSave}</span>
      ) : (
        ""
      )}
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
