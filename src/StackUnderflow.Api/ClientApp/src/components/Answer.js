import React from "react";
import UserDetailsShort from "./UserDetailsShort.js";

const Answer = ({ answer, onStartEditing }) => {
  return (
    <div style={{ borderStyle: "solid", borderColor: "green" }}>
      <hr />
      <div>Voting</div>
      <span>{answer.body}</span>
      <UserDetailsShort
        username={answer.username}
        createdOn={answer.createdOn}
      />
      {answer.isOwner ? <button onClick={onStartEditing}>Edit</button> : ""}
    </div>
  );
};

export default Answer;
