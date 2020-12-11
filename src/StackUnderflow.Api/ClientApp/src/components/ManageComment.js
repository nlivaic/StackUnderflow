import React, { useState } from "react";
import { getErrorMessage } from "../utils/getErrorMessage.js";
import Comment from "./Comment.js";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import * as commentsActions from "../redux/actions/commentsActions.js";
import CommentEdit from "./CommentEdit.js";

const ManageComment = ({
  comment,
  commentsActions,
  parentType,
  parentIds,
  action = "New",
}) => {
  const [isSaving, setIsSaving] = useState(false);
  const [isDeleting, setIsDeleting] = useState(false);
  const [errors, setErrors] = useState({});
  const [isEditingOrNew, setIsEditingOrNew] = useState(
    action === "New" ? true : false
  );
  const [editedComment, setEditedComment] = useState(
    comment
      ? comment
      : {
          body: "",
        }
  );

  const onEditToggleHandle = (e) => {
    e.preventDefault();
    if (comment.isOwner) {
      setErrors({});
      setIsEditingOrNew(!isEditingOrNew);
      setEditedComment(comment);
    }
  };

  const onSaveNewHandle = async (e) => {
    e.preventDefault();
    if (!isFormValid()) {
      return;
    }
    setIsSaving(true);
    try {
      await commentsActions.postComment(editedComment, parentType, parentIds);
      setEditedComment({ body: "" });
    } catch (error) {
      setErrors({ onSave: getErrorMessage(error) });
    }
    setIsSaving(false);
  };

  const onSaveEditHandle = async (e) => {
    e.preventDefault();
    if (!isFormValid()) {
      return;
    }
    setIsSaving(true);
    try {
      await commentsActions.editComment(editedComment, parentType, parentIds);
      setIsEditingOrNew(false);
    } catch (error) {
      setErrors({ onSave: getErrorMessage(error) });
    }
    setIsSaving(false);
  };

  const onDeleteHandle = async (e) => {
    e.preventDefault();
    if (comment.isOwner) {
      setIsDeleting(true);
      try {
        await commentsActions.deleteComment(
          parentType,
          { parentIds },
          editedComment.id
        );
      } catch (error) {
        setIsDeleting(false);
        setErrors({ onDelete: getErrorMessage(error) });
      }
    }
  };
  const onInputChange = ({ target }) => {
    setEditedComment({ ...editedComment, [target.id]: target.value });
  };

  const isFormValid = () => {
    const error = {};
    if (editedComment.body.length < 30) {
      error.body = "Comment's body must be at least 30 characters.";
    }
    setErrors(error);
    return Object.keys(error).length === 0;
  };

  return (
    <div>
      {isEditingOrNew ? (
        <CommentEdit
          comment={editedComment}
          onSaveNewComment={onSaveNewHandle}
          onSaveEditComment={onSaveEditHandle}
          onCancel={onEditToggleHandle}
          action={action}
          isSaving={isSaving}
          onInputChange={onInputChange}
          errors={errors}
        />
      ) : (
        <Comment
          onStartEditing={onEditToggleHandle}
          comment={comment}
          errors={errors}
        />
      )}
    </div>
  );
};

const mapStateToDispatch = (dispatch) => {
  return {
    commentsActions: bindActionCreators(commentsActions, dispatch),
  };
};

export default connect(null, mapStateToDispatch)(ManageComment);
