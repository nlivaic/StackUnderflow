import { combineReducers } from "redux";
import {
  EDIT_COMMENT_SUCCESS,
  LOAD_COMMENT_ON_QUESTION_SUCCESS,
  LOAD_COMMENT_ON_ANSWERS_SUCCESS,
  DELETE_COMMENT_SUCCESS,
  SAVE_COMMENT_ON_QUESTION_SUCCESS,
  SAVE_COMMENT_ON_ANSWER_SUCCESS,
  CLEAR_ALL_COMMENTS,
} from "../actions/actionTypes.js";

const commentsOnQuestionReducer = (state = [], action) => {
  switch (action.type) {
    case LOAD_COMMENT_ON_QUESTION_SUCCESS:
      return action.comments;
    case EDIT_COMMENT_SUCCESS:
      return state.map((comment) =>
        comment.id === action.comment.id ? action.comment : comment
      );
    case DELETE_COMMENT_SUCCESS:
      return state.filter((comment) => comment.id !== action.commentId);
    case SAVE_COMMENT_ON_QUESTION_SUCCESS:
      return [...state, action.comment];
    case CLEAR_ALL_COMMENTS:
      return [];
    default:
      return state;
  }
};

const commentsOnAnswersReducer = (state = [], action) => {
  switch (action.type) {
    case LOAD_COMMENT_ON_ANSWERS_SUCCESS:
      return action.comments;
    case EDIT_COMMENT_SUCCESS:
      return state.map((comment) =>
        comment.id === action.comment.id ? action.comment : comment
      );
    case DELETE_COMMENT_SUCCESS:
      return state.filter((comment) => comment.id !== action.commentId);
    case SAVE_COMMENT_ON_ANSWER_SUCCESS:
      return [...state, action.comment];
    case CLEAR_ALL_COMMENTS:
      return [];
    default:
      return state;
  }
};

export default combineReducers({
  commentsOnQuestion: commentsOnQuestionReducer,
  commentsOnAnswers: commentsOnAnswersReducer,
});

// Combined state selector.
export const getCommentsOnQuestion = (state) => state.commentsOnQuestion;
export const getCommentsOnAnswers = (state) => state.commentsOnAnswers;
export const getIsNoCommentsOnQuestion = (state) =>
  state.isNoCommentsOnQuestion;
