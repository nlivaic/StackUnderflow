import * as answersApi from "../../api/answersApi.js";
import { LOAD_ANSWERS_SUCCESS } from "../actions/actionTypes.js";
import * as apiStatus from "../actions/apiStatusActions.js";

function getAnswersSuccess(answers) {
  return {
    type: LOAD_ANSWERS_SUCCESS,
    answers,
  };
}

export const getAnswers = (questionId) => {
  return async (dispatch) => {
    try {
      debugger;
      dispatch(apiStatus.beginApiCall());
      let data = await answersApi.getAnswers(questionId);
      dispatch(getAnswersSuccess(data));
    } catch (error) {
      debugger;
      dispatch(apiStatus.apiCallError());
      console.log(error);
      throw error;
    }
  };
};
