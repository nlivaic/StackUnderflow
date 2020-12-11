import React from "react";
import UserDetailsShort from "./UserDetailsShort.js";

const Comment = ({ onStartEditing, comment }) => {
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
        <hr />
        <div>Voting</div>
        <span>{comment.body}</span>
        <UserDetailsShort
          username={comment.username}
          createdOn={comment.createdOn}
        />
        {comment.isOwner ? <button onClick={onStartEditing}>Edit</button> : ""}
      </div>
    </div>
  );
};

export default Comment;
