import {
  LOAD_COMMENT_ON_QUESTION_SUCCESS,
  LOAD_COMMENT_ON_ANSWERS_SUCCESS,
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
    debugger;
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

export const clearAllComments = () => {
  return {
    type: CLEAR_ALL_COMMENTS,
  };
};
