import React from "react";
import UserDetails from "./UserDetails.js";

const Question = ({
  title,
  body,
  username,
  hasAcceptedAnswer,
  createdOn,
  tags,
}) => {
  return (
    <div>
      <h3>{title}</h3>
      <span>{body}</span>
      <UserDetails username={username} createdOn={createdOn} />
      <div>{hasAcceptedAnswer ? "Accepted" : ""}</div>
      <div>
        {tags.map((tag) => (
          <span key={tag.name}>{tag.name}</span>
        ))}
      </div>
    </div>
  );
};

export default Question;
