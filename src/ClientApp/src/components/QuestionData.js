import React, { useEffect, useState } from "react";
import ManageQuestion from "./ManageQuestion.js";
import { bindActionCreators } from "redux";
import * as questionActions from "../redux/actions/questionActions.js";
import { connect } from "react-redux";
import { useHistory } from "react-router-dom";
import * as actionTypes from "../utils/actionTypes.js";

const QuestionData = ({ questionActions, questionId }) => {
  const [isLoading, setIsLoading] = useState(false);
  const history = useHistory();

  useEffect(() => {
    questionActions
      .getQuestion(questionId)
      .then((_) => setIsLoading(false))
      .catch((error) => {
        if (error.response.status === 404) {
          history.push("/NotFound");
          return;
        }
      });
    setIsLoading(true);
    // Clear question when leaving Question Page.
    return () => {
      questionActions.clearQuestion();
    };
    // eslint-disable-next-line
  }, []);
  return (
    <div>
      <div>
        {isLoading ? (
          "Loading question..."
        ) : (
          <>
            <ManageQuestion action={actionTypes.ReadAndEdit} />
          </>
        )}
      </div>
    </div>
  );
};

const mapDispatchToProps = (dispatch) => {
  return {
    questionActions: bindActionCreators(questionActions, dispatch),
  };
};

export default connect(null, mapDispatchToProps)(QuestionData);
