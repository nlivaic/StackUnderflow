import { combineReducers } from "redux";
import {
  LOAD_COMMENT_ON_QUESTION_SUCCESS,
  LOAD_COMMENT_ON_ANSWERS_SUCCESS,
  CLEAR_ALL_COMMENTS,
} from "../actions/actionTypes.js";

const commentsOnQuestionReducer = (state = [], action) => {
  switch (action.type) {
    case LOAD_COMMENT_ON_QUESTION_SUCCESS:
      return action.comments;
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
    case CLEAR_ALL_COMMENTS:
      return [];
    default:
      return state;
  }
};

const isNoCommentsOnQuestionReducer = (state = false, action) => {
  switch (action.type) {
    case LOAD_COMMENT_ON_QUESTION_SUCCESS:
      return action.comments.length === 0;
    default:
      return state;
  }
};

export default combineReducers({
  commentsOnQuestion: commentsOnQuestionReducer,
  commentsOnAnswers: commentsOnAnswersReducer,
  isNoCommentsOnQuestion: isNoCommentsOnQuestionReducer,
});

// Combined state selector.
export const getCommentsOnQuestion = (state) => state.commentsOnQuestion;
export const getCommentsOnAnswers = (state) => state.commentsOnAnswers;
export const getIsNoCommentsOnQuestion = (state) =>
  state.isNoCommentsOnQuestion;
