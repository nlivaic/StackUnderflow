import initialState from "./initialState.js";
import {
  CLEAR_QUESTION,
  LOAD_QUESTION_SUCCESS,
  EDIT_QUESTION_SUCCESS,
  ASK_QUESTION_SUCCESS,
  DELETE_QUESTION_SUCCESS,
  CLEAR_REDIRECT_TO_QUESTION,
  ACCEPT_ANSWER_SUCCESS,
  DRAFT_QUESTION,
  CLEAR_DRAFT_QUESTION,
} from "../actions/actionTypes";
import { combineReducers } from "redux";

function questionReducer(state = initialState.question, action) {
  switch (action.type) {
    case CLEAR_QUESTION:
      return initialState.question;
    case LOAD_QUESTION_SUCCESS:
    case ASK_QUESTION_SUCCESS:
    case EDIT_QUESTION_SUCCESS:
      return action.question;
    case DELETE_QUESTION_SUCCESS:
      return initialState.question;
    case ACCEPT_ANSWER_SUCCESS:
      return { ...state, hasAcceptedAnswer: true };
    default:
      return state;
  }
}

function questionDraftReducer(state = initialState.question, action) {
  switch (action.type) {
    case DRAFT_QUESTION:
      return action.question;
    case CLEAR_DRAFT_QUESTION:
      return initialState.question;
    default:
      return state;
  }
}

function redirectToQuestionReducer(state = null, action) {
  switch (action.type) {
    case CLEAR_REDIRECT_TO_QUESTION:
      return null;
    case ASK_QUESTION_SUCCESS:
      return action.question.id;
    default:
      return state;
  }
}

function redirectToHomeReducer(state = false, action) {
  switch (action.type) {
    case CLEAR_REDIRECT_TO_QUESTION:
      return null;
    case DELETE_QUESTION_SUCCESS:
      return true;
    default:
      return state;
  }
}

export default combineReducers({
  question: questionReducer,
  redirectToQuestion: redirectToQuestionReducer,
  redirectToHome: redirectToHomeReducer,
  questionDraft: questionDraftReducer,
});

// Combined state selector.
export const getQuestion = (state) => state.question;
export const getQuestionDraft = (state) => state.questionDraft;
export const getRedirectToQuestion = (state) => state.redirectToQuestion;
export const getRedirectToHome = (state) => state.redirectToHome;
export const getQuestionHasAcceptedAnswer = (state) =>
  state.question.hasAcceptedAnswer;
