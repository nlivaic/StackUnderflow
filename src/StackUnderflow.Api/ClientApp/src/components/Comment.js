import React from "react";
import UserDetailsShort from "./UserDetailsShort.js";

const Comment = ({ body, username, createdOn }) => {
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
        <span>{body}</span>
        <UserDetailsShort username={username} createdOn={createdOn} />
      </div>
    </div>
  );
};

export default Comment;
