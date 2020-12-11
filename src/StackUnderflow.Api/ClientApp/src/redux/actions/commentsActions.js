import {
  LOAD_COMMENT_ON_QUESTION_SUCCESS,
  LOAD_COMMENT_ON_ANSWERS_SUCCESS,
  SAVE_COMMENT_ON_QUESTION_SUCCESS,
  SAVE_COMMENT_ON_ANSWER_SUCCESS,
  EDIT_COMMENT_ON_QUESTION_SUCCESS,
  EDIT_COMMENT_ON_ANSWER_SUCCESS,
  CLEAR_ALL_COMMENTS,
} from "../actions/actionTypes.js";
import * as apiStatusActions from "./apiStatusActions.js";
import * as commentsApi from "../../api/commentsApi.js";

function loadCommentsForQuestionSuccess(comments) {
  return {
    type: LOAD_COMMENT_ON_QUESTION_SUCCESS,
    comments,
  };
}

function loadCommentsForAnswersSuccess(comments) {
  return {
    type: LOAD_COMMENT_ON_ANSWERS_SUCCESS,
    comments,
  };
}

export const getComments = (parentType, parentIds) => {
  return async (dispatch) => {
    dispatch(apiStatusActions.beginApiCall());
    try {
      let data = await commentsApi.getComments(parentType, parentIds);
      switch (parentType) {
        case "question":
          dispatch(loadCommentsForQuestionSuccess(data));
          break;
        case "answer":
          dispatch(loadCommentsForAnswersSuccess(data));
          break;
        default:
          throw new Error("Unknown case: " + parentType);
      }
    } catch (error) {
      console.log(error);
      dispatch(apiStatusActions.apiCallError());
      throw error;
    }
  };
};

function saveCommentForQuestionSuccess(comment) {
  return {
    type: SAVE_COMMENT_ON_QUESTION_SUCCESS,
    comment,
  };
}

function saveCommentForAnswerSuccess(comment) {
  return {
    type: SAVE_COMMENT_ON_ANSWER_SUCCESS,
    comment,
  };
}

export const postComments = (comment, parentType, parentIds) => {
  return async (dispatch) => {
    dispatch(apiStatusActions.beginApiCall());
    try {
      let data = await commentsApi.postComment(comment, parentType, parentIds);
      switch (parentType) {
        case "question":
          dispatch(saveCommentForQuestionSuccess(data));
          break;
        case "answer":
          dispatch(saveCommentForAnswerSuccess(data));
          break;
        default:
          throw new Error("Unknown case: " + parentType);
      }
    } catch (error) {
      console.log(error);
      dispatch(apiStatusActions.apiCallError());
      throw error;
    }
  };
};

function editCommentForQuestionSuccess(comment, parentIds) {
  return {
    type: EDIT_COMMENT_ON_QUESTION_SUCCESS,
    comment,
  };
}

function editCommentForAnswerSuccess(comment, parentIds) {
  return {
    type: EDIT_COMMENT_ON_ANSWER_SUCCESS,
    comment,
  };
}

export const editComment = (comment, parentType, parentIds) => {
  return async (dispatch) => {
    dispatch(apiStatusActions.beginApiCall());
    try {
      await commentsApi.editComment(comment, parentType, parentIds);
      switch (parentType) {
        case "question":
          dispatch(editCommentForQuestionSuccess(comment, parentIds));
          break;
        case "answer":
          dispatch(editCommentForAnswerSuccess(comment, parentIds));
          break;
        default:
          throw new Error("Unknown case: " + parentType);
      }
    } catch (error) {
      console.log(error);
      dispatch(apiStatusActions.apiCallError());
      throw error;
    }
  };
};

export const clearAllComments = () => {
  return {
    type: CLEAR_ALL_COMMENTS,
  };
};
