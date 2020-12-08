import React from "react";
import UserDetailsShort from "./UserDetailsShort.js";

const Answer = ({ answer, onStartEditing, onDelete, isDeleting }) => {
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
      {answer.isOwner ? (
        <button onClick={onDelete} disabled={isDeleting}>
          {isDeleting ? "Deleting..." : "Delete"}
        </button>
      ) : (
        ""
      )}
    </div>
  );
};

export default Answer;
