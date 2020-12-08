import { combineReducers } from "redux";
import {
  LOAD_ANSWERS_SUCCESS,
  EDIT_ANSWER_SUCCESS,
  SAVE_ANSWER_SUCCESS,
  CLEAR_ANSWERS_SUCCESS,
  DELETE_ANSWER_SUCCESS,
} from "../actions/actionTypes.js";

const answersReducer = (state = [], action) => {
  switch (action.type) {
    case LOAD_ANSWERS_SUCCESS:
      return action.answers;
    case SAVE_ANSWER_SUCCESS:
      return [...state, action.answer];
    case EDIT_ANSWER_SUCCESS:
      return state.map((answer) =>
        answer.id === action.answer.id ? action.answer : answer
      );
    case DELETE_ANSWER_SUCCESS:
      return state.filter((answer) => answer.id !== action.answerId);
    case CLEAR_ANSWERS_SUCCESS:
      return [];

    default:
      return state;
  }
};

export default combineReducers({ answers: answersReducer });

// Combine level selectors
export const getAnswers = (state) => state.answers;
