import React from "react";
import UserDetails from "./UserDetails.js";

const Question = ({
  question,
  onStartEditing,
  onDelete,
  isDeleting,
  errors,
}) => {
  return (
    <div style={{ borderStyle: "solid", borderColor: "red" }}>
      {errors.onDelete ? (
        <span style={{ color: "red" }}>{errors.onDelete}</span>
      ) : (
        ""
      )}
      <h3>{question.title}</h3>
      <span>{question.body}</span>
      <UserDetails
        username={question.username}
        createdOn={question.createdOn}
      />
      <div>{question.hasAcceptedAnswer ? "Accepted" : ""}</div>
      <div>
        {question.tags.map((tag) => (
          <span key={tag.name}>{tag.name}</span>
        ))}
      </div>
      {question.isOwner ? <button onClick={onStartEditing}>Edit</button> : ""}
      {question.isOwner ? (
        <button onClick={onDelete} disabled={isDeleting}>
          {isDeleting ? "Deleting..." : "Delete"}
        </button>
      ) : (
        ""
      )}
    </div>
  );
};

export default Question;
