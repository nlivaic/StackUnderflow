import React, { useEffect, useState } from "react";
import * as commentsApi from "../api/commentsApi.js";
import Comment from "./Comment.js";

const CommentsList = ({ parentType, parentId }) => {
  const [isLoaded, setIsLoaded] = useState(false);
  const [commentsList, setCommentsList] = useState([]);
  useEffect(() => {
    const getComments = async () => {
      var data = await commentsApi.getComments(parentType, parentId);
      setCommentsList(data);
      setIsLoaded(true);
    };
    getComments();
  }, []);

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
