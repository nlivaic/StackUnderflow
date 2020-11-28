import React from "react";
import UserDetails from "./UserDetails.js";

const Question = ({
  title,
  body,
  username,
  hasAcceptedAnswer,
  createdOn,
  tags,
  isOwner,
  onEdit,
  onDelete,
  errors,
}) => {
  return (
    <div style={{ borderStyle: "solid", borderColor: "red" }}>
      {errors.onDelete ? (
        <span style={{ color: "red" }}>{errors.onDelete}</span>
      ) : (
        ""
      )}
      <h3>{title}</h3>
      <span>{body}</span>
      <UserDetails username={username} createdOn={createdOn} />
      <div>{hasAcceptedAnswer ? "Accepted" : ""}</div>
      <div>
        {tags.map((tag) => (
          <span key={tag.name}>{tag.name}</span>
        ))}
      </div>
      {isOwner ? (
        <>
          <button onClick={onEdit}>Edit</button>
          <button onClick={onDelete}>Delete</button>
        </>
      ) : (
        ""
      )}
    </div>
  );
};

export default Question;
