import React, { useEffect, useState } from "react";
import ManageAnswer from "./ManageAnswer.js";
import Comment from "./Comment.js";
import { getAnswers } from "../redux/reducers/index.js";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import * as answersActions from "../redux/actions/answersActions.js";
import * as commentsActions from "../redux/actions/commentsActions.js";

const AnswersListData = ({ answers, answersActions, questionId }) => {
  const [isLoading, setIsLoading] = useState(false);
  useEffect(() => {
    answersActions.getAnswers(questionId).then((_) => {
      debugger;
      commentsActions.getComments(
        "answer",
        answers.map((answer) => answer.id)
      );
      setIsLoading(false);
    });
    setIsLoading(true);
  }, [questionId]);

  const renderAnswers = (answers) =>
    answers.length === 0
      ? "No answers"
      : answers.map((answer) => {
          debugger;
          return (
            <div key={answer.id}>
              <ManageAnswer
                key={answer.id}
                answer={answer}
                questionId={questionId}
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
  };
};

const mapStateToDispatch = (dispatch) => {
  return {
    answersActions: bindActionCreators(answersActions, dispatch),
    commentsActions: bindActionCreators(commentsActions, dispatch),
  };
};

export default connect(mapStateToProps, mapStateToDispatch)(AnswersListData);
