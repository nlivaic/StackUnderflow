import React from "react";
import UserDetailsShort from "./UserDetailsShort.js";

const Answer = ({ body, username, createdOn }) => {
  return (
    <div style={{ borderStyle: "solid", borderColor: "green" }}>
      <hr />
      <div>Voting</div>
      <span>{body}</span>
      <UserDetailsShort username={username} createdOn={createdOn} />
    </div>
  );
};

export default Answer;
