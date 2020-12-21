import React, { useEffect, useState } from "react";
import ManageAnswer from "./ManageAnswer.js";
import ManageComment from "./ManageComment.js";
import { getAnswers, getComments } from "../redux/reducers/index.js";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import * as answersActions from "../redux/actions/answersActions.js";
import * as commentsActions from "../redux/actions/commentsActions.js";
import * as actionTypes from "../utils/actionTypes.js";

const AnswersList = ({
  answers,
  comments,
  answersActions,
  commentsActions,
  questionId,
}) => {
  const [isLoading, setIsLoading] = useState(false);
  useEffect(() => {
    const loadAnswers = async () => {
      setIsLoading(true);
      const data = await answersActions.getAnswers(questionId);
      const answersFromState = getAnswers(data);
      if (answersFromState.length > 0) {
        await commentsActions.getComments("answer", {
          questionId,
          answerIds: answersFromState.map((answer) => answer.id),
        });
      }
      setIsLoading(false);
    };
    loadAnswers();
    return () => {
      answersActions.clearAnswers();
    };
    // eslint-disable-next-line
  }, [questionId]);

  const renderAnswers = (answers) =>
    answers.length === 0
      ? "No answers"
      : answers.map((answer) => {
          return (
            <div key={answer.id}>
              <ManageAnswer
                key={answer.id}
                answer={answer}
                questionId={questionId}
              />
              {comments
                .filter((comment) => comment.answerId === answer.id)
                .map((comment) => (
                  <ManageComment
                    key={comment.id}
                    comment={comment}
                    parentType="answer"
                    parentIds={{ questionId, answerId: answer.id }}
                    action={actionTypes.ReadAndEdit}
                  />
                ))}
              <ManageComment
                parentType="answer"
                parentIds={{ questionId, answerId: answer.id }}
                action={actionTypes.New}
              />
            </div>
          );
        });

  return (
    <div>
      <div>{isLoading ? "Loading answers..." : renderAnswers(answers)}</div>
    </div>
  );
};

const mapStateToProps = (state) => {
  return {
    answers: getAnswers(state),
    comments: getComments("answer", state),
  };
};

const mapStateToDispatch = (dispatch) => {
  return {
    answersActions: bindActionCreators(answersActions, dispatch),
    commentsActions: bindActionCreators(commentsActions, dispatch),
  };
};

export default connect(mapStateToProps, mapStateToDispatch)(AnswersList);
