import * as answersApi from "../../api/answersApi.js";
import {
  LOAD_ANSWERS_SUCCESS,
  EDIT_ANSWERS_SUCCESS,
  SAVE_ANSWERS_SUCCESS,
} from "../actions/actionTypes.js";
import * as apiStatus from "../actions/apiStatusActions.js";
import * as fromAnswers from "../reducers/index.js";

function getAnswersSuccess(answers) {
  return {
    type: LOAD_ANSWERS_SUCCESS,
    answers,
  };
}

export const getAnswers = (questionId) => {
  return async (dispatch, getState) => {
    dispatch(apiStatus.beginApiCall());
    try {
      let data = await answersApi.getAnswers(questionId);
      dispatch(getAnswersSuccess(data));
      return getState();
    } catch (error) {
      dispatch(apiStatus.apiCallError());
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
    dispatch(apiStatus.beginApiCall());
    try {
      await answersApi.editAnswer(answer, questionId, answerId);
    } catch (error) {
      dispatch(apiStatus.apiCallError());
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
