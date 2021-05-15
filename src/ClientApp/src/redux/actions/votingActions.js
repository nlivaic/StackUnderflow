import {
    UPVOTE_QUESTION_SUCCESS,
    DOWNVOTE_QUESTION_SUCCESS,
    REVOKE_VOTE_QUESTION_SUCCESS
  } from "./actionTypes";
  import * as apiStatusActions from "./apiStatusActions.js";
  import * as votesApi from "../../api/votesApi.js";
  
  function upvoteQuestionSuccess(vote) {
    return {
      type: UPVOTE_QUESTION_SUCCESS,
      vote,
    };
  }
  
  function downvoteQuestionSuccess(vote) {
    return {
      type: DOWNVOTE_QUESTION_SUCCESS,
      vote,
    };
  }
  
  function revokeVoteQuestionSuccess(voteId, voteType) {
    return {
      type: REVOKE_VOTE_QUESTION_SUCCESS,
      vote: { voteId, voteType},
    };
  }
  
  export function upvoteQuestion(questionId) {
    return async (dispatch) => {
      dispatch(apiStatusActions.beginApiCall());
      try {
        dispatch(
          upvoteQuestionSuccess(
            await votesApi.upvoteQuestion(questionId)));
      } catch (error) {
        console.error(error);
        dispatch(apiStatusActions.apiCallError());
        throw error;
      }
    };
  }
  
  export function downvoteQuestion(questionId) {
    return async (dispatch) => {
      dispatch(apiStatusActions.beginApiCall());
      try {
        dispatch(
          downvoteQuestionSuccess(
            await votesApi.downvoteQuestion(questionId)));
      } catch (error) {
        console.error(error);
        dispatch(apiStatusActions.apiCallError());
        throw error;
      }
    };
  }
  
  export function revokeVoteQuestion(voteId, voteType) {
    return async (dispatch) => {
      dispatch(apiStatusActions.beginApiCall());
      try {
        dispatch(
          revokeVoteQuestionSuccess(
            await votesApi.revokeVote(voteId), voteType));
      } catch (error) {
        console.error(error);
        dispatch(apiStatusActions.apiCallError());
        throw error;
      }
    };
  }
  