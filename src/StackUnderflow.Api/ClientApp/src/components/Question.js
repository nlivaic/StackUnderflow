import React from "react";
import UserDetails from "./UserDetails.js";

const Question = ({ question, onStartEditing }) => {
  return (
    <div style={{ borderStyle: "solid", borderColor: "red" }}>
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
    </div>
  );
};

export default Question;
