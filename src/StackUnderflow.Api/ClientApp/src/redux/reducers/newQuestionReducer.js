import initialState from "./initialState.js";
import { CLEAR_QUESTION, SAVE_QUESTION_SUCCESS } from "../actions/actionTypes";

export default function newQuestionReducer(
  state = initialState.newQuestion,
  action
) {
  switch (action.type) {
    case CLEAR_QUESTION:
    case SAVE_QUESTION_SUCCESS:
      return initialState.newQuestion;
    default:
      return state;
  }
}
