import React from "react";

const Downvote = ({isVoted, onDownvoting, onRevoking}) => {
  return ( 
    <div>
    {
      isVoted ?
        <span
          style={{ backgroundColor: 'red'}}
          onClick={onRevoking}>
            Downvote
        </span> :
        <span
          onClick={onDownvoting}>
            Downvote
        </span>
    }
    </div>
  );
}
 
export default Downvote;