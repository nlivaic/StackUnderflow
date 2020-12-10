import * as answersApi from "../../api/answersApi.js";
import {
  LOAD_ANSWERS_SUCCESS,
  EDIT_ANSWER_SUCCESS,
  SAVE_ANSWER_SUCCESS,
  DELETE_ANSWER_SUCCESS,
  ACCEPT_ANSWER_SUCCESS,
  CLEAR_ALL_ANSWERS,
} from "../actions/actionTypes.js";
import * as apiStatusActions from "../actions/apiStatusActions.js";
import * as fromAnswers from "../reducers/index.js";

function getAnswersSuccess(answers) {
  return {
    type: LOAD_ANSWERS_SUCCESS,
    answers,
  };
}

export const getAnswers = (questionId) => {
  return async (dispatch, getState) => {
    dispatch(apiStatusActions.beginApiCall());
    try {
      let data = await answersApi.getAnswers(questionId);
      dispatch(getAnswersSuccess(data));
      return getState();
    } catch (error) {
      dispatch(apiStatusActions.apiCallError());
      throw error;
    }
  };
};

function editAnswersSuccess(answer) {
  return {
    type: EDIT_ANSWER_SUCCESS,
    answer,
  };
}

export const editAnswer = (answer, questionId, answerId) => {
  return async (dispatch, getState) => {
    dispatch(apiStatusActions.beginApiCall());
    try {
      await answersApi.editAnswer(answer, questionId, answerId);
    } catch (error) {
      dispatch(apiStatusActions.apiCallError());
      throw error;
    }
    dispatch(
      editAnswersSuccess({
        ...fromAnswers.getAnswers(getState()),
        ...answer,
      })
    );
  };
};

function postAnswerSuccess(answer) {
  return {
    type: SAVE_ANSWER_SUCCESS,
    answer,
  };
}

export const postAnswer = (answer, questionId) => {
  return async (dispatch) => {
    dispatch(apiStatusActions.beginApiCall());
    try {
      let data = await answersApi.postAnswer(answer, questionId);
      dispatch(postAnswerSuccess(data));
    } catch (error) {
      dispatch(apiStatusActions.apiCallError());
      throw error;
    }
  };
};

function deleteAnswerSuccess(questionId, answerId) {
  return {
    type: DELETE_ANSWER_SUCCESS,
    questionId,
    answerId,
  };
}

export const deleteAnswer = (questionId, answerId) => {
  return async (dispatch) => {
    dispatch(apiStatusActions.beginApiCall());
    try {
      await answersApi.deleteAnswer(questionId, answerId);
      dispatch(deleteAnswerSuccess(questionId, answerId));
    } catch (error) {
      dispatch(apiStatusActions.apiCallError());
      throw error;
    }
  };
};

function acceptAnswerSuccess(questionId, answer) {
  return {
    type: ACCEPT_ANSWER_SUCCESS,
    questionId,
    answer,
  };
}

export const acceptAnswer = (questionId, answerId) => {
  return async (dispatch) => {
    dispatch(apiStatusActions.beginApiCall());
    try {
      let response = await answersApi.acceptAnswer(questionId, answerId);
      dispatch(acceptAnswerSuccess(questionId, response));
    } catch (error) {
      dispatch(apiStatusActions.apiCallError());
      throw error;
    }
  };
};

export const clearAnswers = () => {
  return {
    type: CLEAR_ALL_ANSWERS,
  };
};
