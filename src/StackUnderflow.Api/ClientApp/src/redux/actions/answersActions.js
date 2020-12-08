import * as answersApi from "../../api/answersApi.js";
import {
  LOAD_ANSWERS_SUCCESS,
  EDIT_ANSWERS_SUCCESS,
  SAVE_ANSWER_SUCCESS,
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
      console.log(error);
      throw error;
    }
  };
};

function editAnswersSuccess(answer) {
  return {
    type: EDIT_ANSWERS_SUCCESS,
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
      console.error(error);
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
      console.error(error);
      dispatch(apiStatusActions.apiCallError());
      throw error;
    }
  };
};
