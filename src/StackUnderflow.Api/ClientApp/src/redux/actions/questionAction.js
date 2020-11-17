import { CLEAR_QUESTION, SAVE_QUESTION_SUCCESS } from "./actionTypes";
import * as apiStatusActions from "./apiStatusActions.js";
import questionApi from "../../api/questionApi.js";

export function clearQuestion() {
  return { type: CLEAR_QUESTION };
}

export function askQuestion(question) {
  return (dispatch) => {
    dispatch(apiStatusActions.beginApiCall());
    try {
        await questionApi.askQuestion(question);
        dispatch({type: CLEAR_QUESTION});
    }
    catch (error) {
        dispatch(apiStatusActions.apiCallError());
        console.log(error);
    }
    dispatch({type: SAVE_QUESTION_SUCCESS});
  };
}
