import { combineReducers } from "redux";
import apiCallsInProgress from "./apiStatusReducer.js";
import questionReducer, * as fromQuestion from "./questionReducer.js";
import commentsReducer, * as fromComments from "./commentsReducer.js";
import answersReducer, * as fromAnswers from "./answersReducer.js";
import profileReducer from "./profileReducer.js";

const rootReducer = combineReducers({
  apiCallsInProgress,
  question: questionReducer,
  comments: commentsReducer,
  answers: answersReducer,
  profile: profileReducer,
});

export default rootReducer;

// Top level selectors - Question
export const getQuestion = (state) => fromQuestion.getQuestion(state.question);
export const getRedirectToQuestion = (state) =>
  fromQuestion.getRedirectToQuestion(state.question);
export const getApiCallsInProgress = (state) => state.apiCallsInProgress;
export const getRedirectToHome = (state) =>
  fromQuestion.getRedirectToHome(state.question);
export const getQuestionHasAcceptedAnswer = (state) =>
  fromQuestion.getQuestionHasAcceptedAnswer(state.question);
export const getQuestionDraft = (state) =>
  fromQuestion.getQuestionDraft(state.question);
export const getVoteOnQuestion = (state) =>
  fromQuestion.getVoteOnQuestion(state.question);
  export const getVotesSum = (state) =>
    fromQuestion.getVotesSum(state.question);
// Top level selectors - Comments
export const getComments = (parentType, state) => {
  switch (parentType) {
    case "question":
      return fromComments.getCommentsOnQuestion(state.comments);
    case "answer":
      return fromComments.getCommentsOnAnswers(state.comments);
    default:
      throw new Error("Unknown case: " + parentType);
  }
};
export const getIsNoCommentsOnQuestion = (state) =>
  fromComments.getIsNoCommentsOnQuestion(state.comments);

// Top level selectors: Answers
export const getAnswers = (state) => fromAnswers.getAnswers(state.answers);

// Top level state selector: Profile
export const getProfile = (state) => state.profile;
