import React, { useState } from "react";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import * as votingActions from "../redux/actions/votingActions.js";
import { getVoteOnQuestion, getVotesSum } from "../redux/reducers/index.js";
import { getErrorMessage } from "../utils/getErrorMessage.js";
import Upvote from "./Upvote";
import Downvote from "./Downvote";

const VoteData = ({votingActions, targetId, targetType, vote, votesSum}) => {
  const [errors, setErrors] = useState({});

  const onUpvoting = async e => {
        e.preventDefault();
        try {
            await upvote(targetId, targetType);
        } catch (error) {
            setErrors({ apiError: getErrorMessage(error) });
        }
    }

    const onDownvoting = async e => {
          e.preventDefault();
          try {
              await downvote(targetId, targetType);
          } catch (error) {
              setErrors({ apiError: getErrorMessage(error) });
          }
      }

    const onRevoking = async e => {
        e.preventDefault();
        try {
            await revokeVote(vote.voteId, vote.voteType, targetType);
        } catch (error) {
            setErrors({ apiError: getErrorMessage(error) });
        }
    }

    const upvote = async (targetId, targetType) => {
        switch (targetType) {
            case 'question':
                await votingActions.upvoteQuestion(targetId);
                break;
            default:
                throw new Error(`Unknown vote target type ${targetType}.`);
        }
    };

    const downvote = async (targetId, targetType) => {
        switch (targetType) {
            case 'question':
                await votingActions.downvoteQuestion(targetId);
                break;
            default:
                throw new Error(`Unknown vote target type ${targetType}.`);
        }
    };

    const revokeVote = async (voteId, voteType, targetType) => {
        switch (targetType) {
            case 'question':
                await votingActions.revokeVoteQuestion(voteId, voteType);
                break;
            default:
                throw new Error(`Unknown vote target type ${targetType}.`);
        }
    };

    return (
        <div style={{ borderStyle: "solid", borderColor: "gray"}}>
            {errors.apiError ? (
                <span style={{ color: "red" }}>{errors.apiError}</span>
            ) : (
                ""
            )}
            <Upvote
                isVoted={vote.voteType === 'Upvote'}
                onUpvoting={onUpvoting}
                onRevoking={onRevoking}>
            </Upvote>
            <span>{votesSum}</span>
            <Downvote
                isVoted={vote.voteType === 'Downvote'}
                onDownvoting={onDownvoting}
                onRevoking={onRevoking}>
            </Downvote>
        </div>
      );
}

const mapStateToProps = (state) => {
    return {
        vote: getVoteOnQuestion(state),
        votesSum: getVotesSum(state)
    };
  };

const mapDispatchToProps = (dispatch) => {
    return {
        votingActions: bindActionCreators(votingActions, dispatch),
    };
  };
 
export default connect(mapStateToProps, mapDispatchToProps)(VoteData);