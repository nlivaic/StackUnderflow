import {
  CLEAR_QUESTION,
  CLEAR_REDIRECT_TO_QUESTION,
  ASK_QUESTION_SUCCESS,
  LOAD_QUESTION_SUCCESS,
  EDIT_QUESTION_SUCCESS,
  DELETE_QUESTION_SUCCESS,
  DRAFT_QUESTION,
  CLEAR_DRAFT_QUESTION,
} from "./actionTypes";
import * as apiStatusActions from "./apiStatusActions.js";
import * as questionApi from "../../api/questionApi.js";
import initialState from "../reducers/initialState.js";

export function clearQuestion() {
  return { type: CLEAR_QUESTION };
}

export function clearRedirectToQuestion() {
  return { type: CLEAR_REDIRECT_TO_QUESTION };
}

export function setDraftQuestion(question) {
  return {
    type: DRAFT_QUESTION,
    question,
  };
}

export function clearDraftQuestion() {
  return {
    type: CLEAR_DRAFT_QUESTION,
  };
}

function saveQuestionSuccess(question) {
  return {
    type: ASK_QUESTION_SUCCESS,
    question,
  };
}

export function saveQuestion(question) {
  return async (dispatch) => {
    dispatch(apiStatusActions.beginApiCall());
    try {
      const savedQuestion = await questionApi.askQuestion(question);
      if (savedQuestion.vote === null) {
        savedQuestion.vote = initialState.vote;
      }
      dispatch(saveQuestionSuccess(savedQuestion));
    } catch (error) {
      console.error(error);
      dispatch(apiStatusActions.apiCallError());
      throw error;
    }
  };
}

function loadQuestionSuccess(question) {
  return {
    type: LOAD_QUESTION_SUCCESS,
    question,
  };
}

export function getQuestion(id) {
  return async (dispatch) => {
    dispatch(apiStatusActions.beginApiCall());
    try {
      const question = await questionApi.getQuestion(id);
      if (question.vote === null) {
        question.vote = initialState.vote;
      }
      dispatch(loadQuestionSuccess(question));
    } catch (error) {
      dispatch(apiStatusActions.apiCallError());
      throw error;
    }
  };
}

function editQuestionSuccess(question) {
  return { type: EDIT_QUESTION_SUCCESS, question };
}

export function editQuestion(question) {
  return async (dispatch, getState) => {
    dispatch(apiStatusActions.beginApiCall());
    try {
      await questionApi.editQuestion(question.id, question);
      dispatch(editQuestionSuccess({ ...getState().question, ...question }));
    } catch (error) {
      dispatch(apiStatusActions.apiCallError());
      throw error;
    }
  };
}

function deleteQuestionSuccess(questionId) {
  return { type: DELETE_QUESTION_SUCCESS, questionId };
}

export const deleteQuestion = (questionId) => {
  return async (dispatch) => {
    dispatch(apiStatusActions.beginApiCall());
    try {
      await questionApi.deleteQuestion(questionId);
      dispatch(deleteQuestionSuccess(questionId));
    } catch (error) {
      dispatch(apiStatusActions.apiCallError());
      throw error;
    }
  };
};
