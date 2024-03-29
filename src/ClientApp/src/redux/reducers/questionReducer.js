import initialState from "./initialState.js";
import {
  CLEAR_QUESTION,
  LOAD_QUESTION_SUCCESS,
  EDIT_QUESTION_SUCCESS,
  ASK_QUESTION_SUCCESS,
  DELETE_QUESTION_SUCCESS,
  CLEAR_REDIRECT_TO_QUESTION,
  ACCEPT_ANSWER_SUCCESS,
  DRAFT_QUESTION,
  CLEAR_DRAFT_QUESTION,
  UPVOTE_QUESTION_SUCCESS,
  DOWNVOTE_QUESTION_SUCCESS,
  REVOKE_VOTE_QUESTION_SUCCESS
} from "../actions/actionTypes";
import { combineReducers } from "redux";

function questionReducer(state = initialState.question, action) {
  switch (action.type) {
    case CLEAR_QUESTION:
      return initialState.question;
    case LOAD_QUESTION_SUCCESS:
    case ASK_QUESTION_SUCCESS:
    case EDIT_QUESTION_SUCCESS:
      return action.question;
    case DELETE_QUESTION_SUCCESS:
      return initialState.question;
    case ACCEPT_ANSWER_SUCCESS:
      return { ...state, hasAcceptedAnswer: true };
    case UPVOTE_QUESTION_SUCCESS:
      return { ...state,
        votesSum: state.votesSum + 1,
        vote: {
          id: action.vote.id,
          voteType: action.vote.voteType,
          targetId: action.vote.targetId
        }};
    case DOWNVOTE_QUESTION_SUCCESS:
      return { ...state,
        votesSum: state.votesSum - 1,
        vote: {
          id: action.vote.id,
          voteType: action.vote.voteType,
          targetId: action.vote.targetId
        }
      };
    case REVOKE_VOTE_QUESTION_SUCCESS:
      return {
        ...state,
        votesSum: state.votesSum + (
          action.vote.voteType === 'Upvote' ? -1 : 1
        ),
          vote: initialState.vote
      };
    default:
      return state;
  }
}

function questionDraftReducer(state = initialState.question, action) {
  switch (action.type) {
    case DRAFT_QUESTION:
      return action.question;
    case CLEAR_DRAFT_QUESTION:
      return initialState.question;
    default:
      return state;
  }
}

function redirectToQuestionReducer(state = null, action) {
  switch (action.type) {
    case CLEAR_REDIRECT_TO_QUESTION:
      return null;
    case ASK_QUESTION_SUCCESS:
      return action.question.id;
    default:
      return state;
  }
}

function redirectToHomeReducer(state = false, action) {
  switch (action.type) {
    case CLEAR_REDIRECT_TO_QUESTION:
      return null;
    case DELETE_QUESTION_SUCCESS:
      return true;
    default:
      return state;
  }
}

export default combineReducers({
  question: questionReducer,
  redirectToQuestion: redirectToQuestionReducer,
  redirectToHome: redirectToHomeReducer,
  questionDraft: questionDraftReducer,
});

// Combined state selector.
export const getQuestion = (state) => state.question;
export const getQuestionDraft = (state) => state.questionDraft;
export const getRedirectToQuestion = (state) => state.redirectToQuestion;
export const getRedirectToHome = (state) => state.redirectToHome;
export const getQuestionHasAcceptedAnswer = (state) =>
  state.question.hasAcceptedAnswer;
export const getVoteOnQuestion = (state) => {
  return { 
    voteId: state.question.vote.id,
    voteType: state.question.vote.voteType,
    voteTargetId: state.question.vote.targetId
  };
};
export const getVotesSum = (state) => state.question.votesSum;
