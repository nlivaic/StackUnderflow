import React from "react";
import UserDetailsShort from "./UserDetailsShort.js";

const Comment = ({ onStartEditing, isDeleting, onDelete, comment, errors }) => {
  return (
    <div
      style={{
        width: "500px",
      }}
    >
      <div
        style={{
          marginLeft: "50px",
          borderStyle: "solid",
          borderColor: "gray",
        }}
      >
        {errors.apiError ? (
          <span style={{ color: "red" }}>{errors.apiError}</span>
        ) : (
            ""
          )}
        <hr />
        <div>Voting</div>
        <span>{comment.body}</span>
        <UserDetailsShort
          username={comment.username}
          createdOn={comment.createdOn}
        />
        {(comment.isOwner || comment.isModerator) ? <button onClick={onStartEditing}>Edit</button> : ""}
        {comment.isOwner ? (
          <button onClick={onDelete}>
            {isDeleting ? "Deleting..." : "Delete"}
          </button>
        ) : (
            ""
          )}
      </div>
    </div>
  );
};

export default Comment;
