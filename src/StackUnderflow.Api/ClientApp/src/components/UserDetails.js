import React from "react";

const UserDetails = ({ username, createdOn }) => {
  return (
    <div>
      <span>
        Asked by {username} on {createdOn}
      </span>
    </div>
  );
};

export default UserDetails;
