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
    case SAVE_ANSWERS_SUCCESS:
      return [...state.answers, action.answer];
    case EDIT_ANSWERS_SUCCESS:
      return state.map((answer) =>
        answer.id === action.answer.id ? action.answer : answer
      );
    case CLEAR_ANSWERS_SUCCESS:
      return [];

    default:
      return state;
  }
};

export default combineReducers({ answers: answersReducer });

// Combine level selectors
export const getAnswers = (state) => state.answers;
