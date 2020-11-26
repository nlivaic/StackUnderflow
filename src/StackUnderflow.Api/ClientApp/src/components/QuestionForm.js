import React from "react";

const QuestionForm = ({ onInputChange, onSubmit, isSaving, errors }) => {
  return (
    <div>
      <input
        type="text"
        onChange={onInputChange}
        id="title"
        placeholder="Title..."
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
      />
      {errors.body ? <span style={{ color: "red" }}>* {errors.body}</span> : ""}
      <br />
      <input
        type="text"
        onChange={onInputChange}
        id="tagIds"
        placeholder="Tags..."
      />
      {errors.tagIds ? (
        <span style={{ color: "red" }}>* {errors.tagIds}</span>
      ) : (
        ""
      )}
      <br />
      <button type="submit" onClick={onSubmit} disabled={isSaving}>
        {isSaving ? "Saving..." : "Ask Question"}
      </button>
    </div>
  );
};

export default QuestionForm;
