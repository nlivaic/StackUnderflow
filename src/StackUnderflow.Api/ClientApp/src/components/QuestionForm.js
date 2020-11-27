import React from "react";

const QuestionForm = ({
  question,
  onInputChange,
  onSubmit,
  isSaving,
  buttonText,
  errors,
}) => {
  return (
    <div>
      {errors.onSave ? (
        <>
          <span style={{ color: "red" }}>* {errors.onSave}</span>
          <br />
        </>
      ) : (
        <br />
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
        id="tagIds"
        placeholder="Tags..."
        value={question.tagIds.join(" ")}
      />
      {errors.tagIds ? (
        <span style={{ color: "red" }}>* {errors.tagIds}</span>
      ) : (
        ""
      )}
      <br />
      <button type="submit" onClick={onSubmit} disabled={isSaving}>
        {isSaving ? "Saving..." : buttonText}
      </button>
    </div>
  );
};

export default QuestionForm;
