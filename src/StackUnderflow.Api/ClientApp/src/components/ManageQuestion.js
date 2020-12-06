import React, { useEffect, useState } from "react";
import { useHistory } from "react-router-dom";
import Question from "./Question.js";
import QuestionEdit from "./QuestionEdit.js";
import { getErrorMessage } from "../utils/getErrorMessage.js";
import { getQuestion, getRedirectToQuestion } from "../redux/reducers/index.js";
import * as questionActions from "../redux/actions/questionActions.js";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";

const ManageQuestion = ({
  questionActions,
  question,
  redirectToQuestion,
  action = "New",
}) => {
  const [isSaving, setIsSaving] = useState(false);
  const [errors, setErrors] = useState({});
  const [isEditingOrNew, setIsEditingOrNew] = useState(
    action === "New" ? true : false
  );
  const [editedQuestion, setEditedQuestion] = useState(
    action === "New"
      ? {
          title: "",
          body: "",
          tags: [],
        }
      : question
  );
  const history = useHistory();

  useEffect(() => {
    if (redirectToQuestion) {
      history.push(`/questions/${redirectToQuestion}`);
      return;
    }
    // Clear the id of question you want to redirect.
    // Otherwise you would get redirected if you wanted to post two
    // questions one after another (i.e. without going to another page).
    return () => {
      questionActions.clearRedirectToQuestion();
    };
  }, [redirectToQuestion]);

  const onEditToggleHandle = (e) => {
    e.preventDefault();
    if (question.isOwner) {
      setIsEditingOrNew(!isEditingOrNew);
    }
  };

  const onSaveNewHandle = async (e) => {
    e.preventDefault();
    questionActions.saveQuestion(editedQuestion).catch((error) => {
      setIsSaving(false);
      setErrors({ onSave: getErrorMessage(error) });
    });
    setIsSaving(true);
  };

  const onSaveEditHandle = async (e) => {
    e.preventDefault();

    questionActions
      .editQuestion(editedQuestion)
      .then((_) => {
        setIsSaving(false);
        setIsEditingOrNew(false);
      })
      .catch((error) => {
        setIsSaving(false);
        setErrors({ onSave: getErrorMessage(error) });
      });
    setIsSaving(true);
  };

  const onInputChange = ({ target }) => {
    let value =
      target.id === "tags"
        ? target.value.split(" ").map((t) => {
            return { id: t };
          })
        : target.value;
    setEditedQuestion({ ...editedQuestion, [target.id]: value });
  };
  return (
    <div>
      {isEditingOrNew ? (
        <QuestionEdit
          question={editedQuestion}
          onSaveNewQuestion={onSaveNewHandle}
          onSaveEditQuestion={onSaveEditHandle}
          onCancel={onEditToggleHandle}
          action={action}
          isSaving={isSaving}
          onInputChange={onInputChange}
          errors={errors}
        />
      ) : (
        <Question
          question={editedQuestion}
          onStartEditing={onEditToggleHandle}
        />
      )}
    </div>
  );
};

const mapStateToProps = (state) => {
  return {
    question: getQuestion(state),
    redirectToQuestion: getRedirectToQuestion(state),
  };
};

const mapDispatchToProps = (dispatch) => {
  return {
    questionActions: bindActionCreators(questionActions, dispatch),
  };
};

export default connect(mapStateToProps, mapDispatchToProps)(ManageQuestion);
