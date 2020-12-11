import React from "react";

const CommentEdit = ({
  comment,
  action,
  onSaveNewComment,
  onSaveEditComment,
  onInputChange,
  isSaving,
  onCancel,
  errors,
}) => {
  const getSaveButton = () => {
    switch (action) {
      case "New":
        return (
          <button onClick={onSaveNewComment} disabled={isSaving}>
            {isSaving ? "Posting..." : "Post Your Comment"}
          </button>
        );
      case "ReadAndEdit":
        return (
          <button onClick={onSaveEditComment} disabled={isSaving}>
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
        placeholder="Enter omment here..."
        value={comment.body}
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

export default CommentEdit;
