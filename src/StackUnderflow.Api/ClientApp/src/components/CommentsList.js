import React, { useEffect, useState } from "react";
import * as commentsApi from "../api/commentsApi.js";
import Comment from "./Comment.js";

const CommentsList = ({ parentType, parentIds }) => {
  const [isLoaded, setIsLoaded] = useState(false);
  const [commentsList, setCommentsList] = useState([]);
  useEffect(() => {
    const getComments = async () => {
      var data = await commentsApi.getComments(parentType, parentIds);
      setCommentsList(data);
      setIsLoaded(true);
    };
    getComments();
  }, [parentType, parentIds]);

  return (
    <div>
      <div>
        {!isLoaded
          ? "Loading comments..."
          : commentsList.map((comment) => (
              <Comment key={comment.id} {...comment} />
            ))}
      </div>
    </div>
  );
};

export default CommentsList;
