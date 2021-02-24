import React from "react";
import UserDetailsShort from "./UserDetailsShort.js";

const Answer = ({
  answer,
  onStartEditing,
  onDelete,
  isDeleting,
  isAcceptable,
  isAccepting,
  onAccept,
  errors,
}) => {
  return (
    <div style={{ borderStyle: "solid", borderColor: "green" }}>
      {errors.apiError ? (
        <span style={{ color: "red" }}>{errors.apiError}</span>
      ) : (
          ""
        )}
      <hr />
      <div>Voting</div>
      {answer.isAcceptedAnswer ? (
        <h5 style={{ color: "green" }}>Accepted</h5>
      ) : (
          ""
        )}
      <span>{answer.body}</span>
      <UserDetailsShort
        username={answer.username}
        createdOn={answer.createdOn}
      />
      {(answer.isOwner || answer.isModerator) ? <button onClick={onStartEditing}>Edit</button> : ""}
      {answer.isOwner ? (
        <button onClick={onDelete} disabled={isDeleting}>
          {isDeleting ? "Deleting..." : "Delete"}
        </button>
      ) : (
          ""
        )}
      {isAcceptable ? (
        <button onClick={onAccept} disabled={isAccepting}>
          {isAccepting ? "Accepting..." : "Accept"}
        </button>
      ) : (
          ""
        )}
    </div>
  );
};

export default Answer;
