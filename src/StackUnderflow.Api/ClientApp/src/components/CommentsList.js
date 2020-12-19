import React, { useEffect, useState } from "react";
import * as commentsActions from "../redux/actions/commentsActions.js";
import ManageComment from "./ManageComment.js";
import { getComments } from "../redux/reducers/index.js";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import * as actionTypes from "../utils/actionTypes.js";

const CommentsList = ({ commentsActions, comments, parentType, parentIds }) => {
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    const getComments = async () => {
      setIsLoading(true);
      await commentsActions.getComments(parentType, parentIds);
      setIsLoading(false);
    };
    getComments();
    // Prevent question edit from reloading the comments.
    // eslint-disable-next-line
    return () => {
      commentsActions.clearAllComments();
    };
    // eslint-disable-next-line
  }, []);

  return (
    <div>
      <div>
        {isLoading
          ? "Loading comments..."
          : comments.map((comment) => (
              <ManageComment
                key={comment.id}
                parentType={parentType}
                parentIds={parentIds}
                action={actionTypes.ReadAndEdit}
                comment={comment}
              />
            ))}
      </div>
    </div>
  );
};

const mapStateToProps = (state, ownProps) => {
  return {
    comments: getComments(ownProps.parentType, state),
  };
};

const mapDispatchToProps = (dispatch) => {
  return {
    commentsActions: bindActionCreators(commentsActions, dispatch),
  };
};

export default connect(mapStateToProps, mapDispatchToProps)(CommentsList);
