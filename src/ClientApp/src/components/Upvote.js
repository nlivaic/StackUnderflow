import React from "react";

const Upvote = ({isVoted, onUpvoting, onRevoking}) => {
  return ( 
    <div>
    {
      isVoted ?
        <span
          style={{ backgroundColor: 'green'}}
          onClick={onRevoking}>
            Upvote
        </span> :
        <span
          onClick={onUpvoting}>
            Upvote
        </span>
    }
    </div>
  );
}
 
export default Upvote;