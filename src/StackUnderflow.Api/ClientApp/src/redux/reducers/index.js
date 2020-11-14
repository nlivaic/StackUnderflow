import { combineReducers } from "redux";
import apiCallsInProgress from "./apiStatusReducer.js";
import newQuestion from "./newQuestionReducer.js";

const rootReducer = combineReducers({ apiCallsInProgress, newQuestion });

export default rootReducer;
