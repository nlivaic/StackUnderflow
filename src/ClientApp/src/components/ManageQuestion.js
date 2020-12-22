import React, { useCallback, useEffect, useState } from "react";
import { useHistory } from "react-router-dom";
import Question from "./Question.js";
import QuestionEdit from "./QuestionEdit.js";
import { getErrorMessage } from "../utils/getErrorMessage.js";
import {
  getQuestion,
  getQuestionDraft,
  getRedirectToQuestion,
  getRedirectToHome,
} from "../redux/reducers/index.js";
import * as questionActions from "../redux/actions/questionActions.js";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import * as actionTypes from "../utils/actionTypes.js";
import throttle from "lodash.throttle";
import initialState from "../redux/reducers/initialState.js";

const ManageQuestion = ({
  questionActions,
  question,
  questionDraft,
  redirectToQuestion,
  redirectToHome,
  action = actionTypes.New,
}) => {
  const [isSaving, setIsSaving] = useState(false);
  const [isDeleting, setIsDeleting] = useState(false);
  const [errors, setErrors] = useState({});
  const [isEditingOrNew, setIsEditingOrNew] = useState(
    action === actionTypes.New ? true : false
  );
  const [editedQuestion, setEditedQuestion] = useState(
    action === actionTypes.New ? questionDraft : question
  );
  const history = useHistory();
  // Makes sure we don't overwhelm Redux store.
  const setQuestionDraft = useCallback(
    throttle((data) => {
      questionActions.setDraftQuestion(data);
    }, 1000),
    []
  );

  useEffect(() => {
    if (redirectToQuestion) {
      history.push(`/questions/${redirectToQuestion}`);
      return;
    }
    if (redirectToHome) {
      history.push("/");
      return;
    }
    // Clear the id of question you want to redirect.
    // Otherwise you would get redirected if you wanted to post two
    // questions one after another (i.e. without going to another page).
    return () => {
      questionActions.clearRedirectToQuestion();
    };
    // eslint-disable-next-line
  }, [redirectToQuestion, redirectToHome]);

  const onEditToggleHandle = (e) => {
    e.preventDefault();
    setErrors({});
    setIsEditingOrNew(!isEditingOrNew);
    setEditedQuestion(question);
  };

  const onSaveNewHandle = async (e) => {
    e.preventDefault();
    setIsSaving(true);
    try {
      await questionActions.saveQuestion(editedQuestion);
    } catch (error) {
      setIsSaving(false);
      setErrors({ apiError: getErrorMessage(error) });
    }
    questionActions.clearDraftQuestion();
  };

  const onSaveEditHandle = async (e) => {
    e.preventDefault();
    if (!isFormValid()) {
      return;
    }
    questionActions
      .editQuestion(editedQuestion)
      .then((_) => {
        setIsSaving(false);
        setIsEditingOrNew(false);
      })
      .catch((error) => {
        setIsSaving(false);
        setErrors({ apiError: getErrorMessage(error) });
      });
    setIsSaving(true);
  };

  const onDeleteHandle = async (e) => {
    e.preventDefault();
    questionActions
      .deleteQuestion(question.id)
      .then((_) => setIsDeleting(false))
      .catch((error) => {
        setIsDeleting(false);
        setErrors({ apiError: getErrorMessage(error) });
      });
    setIsDeleting(true);
  };

  const onClearHandle = async (e) => {
    e.preventDefault();
    questionActions.clearDraftQuestion();
    setEditedQuestion(initialState.question);
  };

  const onInputChange = ({ target }) => {
    let value =
      target.id === "tags"
        ? target.value.split(" ").map((t) => {
            return { id: t };
          })
        : target.value;
    let newEditedQuestion = { ...editedQuestion, [target.id]: value };
    setEditedQuestion(newEditedQuestion);
    if (action === actionTypes.New) {
      setQuestionDraft(newEditedQuestion);
    }
  };

  const isFormValid = () => {
    const error = {};
    if (editedQuestion.title.trim().length === 0) {
      error.title = "Question title not provided.";
    }
    if (editedQuestion.body.trim().length < 100) {
      error.body = "Question body must be at least 100 characters.";
    }
    if (editedQuestion.tags.length === 0) {
      error.tags = "At least one tag must be selected.";
    }
    setErrors(error);
    return Object.keys(error).length === 0;
  };

  return (
    <div>
      {isEditingOrNew ? (
        <QuestionEdit
          question={editedQuestion}
          onSaveNewQuestion={onSaveNewHandle}
          onSaveEditQuestion={onSaveEditHandle}
          onCancel={
            action === actionTypes.New ? onClearHandle : onEditToggleHandle
          }
          action={action}
          isSaving={isSaving}
          onInputChange={onInputChange}
          onClear={onClearHandle}
          errors={errors}
        />
      ) : (
        <>
          <Question
            question={question}
            onStartEditing={onEditToggleHandle}
            onDelete={onDeleteHandle}
            isDeleting={isDeleting}
            errors={errors}
          />
        </>
      )}
    </div>
  );
};

const mapStateToProps = (state) => {
  return {
    question: getQuestion(state),
    questionDraft: getQuestionDraft(state),
    redirectToQuestion: getRedirectToQuestion(state),
    redirectToHome: getRedirectToHome(state),
  };
};

const mapDispatchToProps = (dispatch) => {
  return {
    questionActions: bindActionCreators(questionActions, dispatch),
  };
};

export default connect(mapStateToProps, mapDispatchToProps)(ManageQuestion);
