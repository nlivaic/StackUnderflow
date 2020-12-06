import { combineReducers } from "redux";
import {
  LOAD_ANSWERS_SUCCESS,
  EDIT_ANSWERS_SUCCESS,
  SAVE_ANSWERS_SUCCESS,
  CLEAR_ANSWERS_SUCCESS,
} from "../actions/actionTypes.js";

const answersReducer = (state = [], action) => {
  switch (action.type) {
    case LOAD_ANSWERS_SUCCESS:
      return action.answers;
    case CLEAR_ANSWERS_SUCCESS:
      return [];
    default:
      return state;
  }
};

export default combineReducers({ answers: answersReducer });

// Combine level selectors
export const getAnswers = (state) => state.answers;
