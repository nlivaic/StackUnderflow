import React from "react";

const UserDetailsShort = ({ username, createdOn }) => {
  return (
    <div>
      <span>
        {username} on {createdOn}
      </span>
    </div>
  );
};

export default UserDetailsShort;
