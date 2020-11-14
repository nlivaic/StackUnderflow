import React from "react";
import UserDetailsShort from "./UserDetailsShort.js";

const Comment = ({ body, username, createdOn }) => {
  return (
    <div>
      <hr />
      <div>Voting</div>
      <span>{body}</span>
      <UserDetailsShort username={username} createdOn={createdOn} />
    </div>
  );
};

export default Comment;
