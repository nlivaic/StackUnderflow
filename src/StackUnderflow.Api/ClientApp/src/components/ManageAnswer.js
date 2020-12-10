import React, { useState } from "react";
import Answer from "./Answer.js";
import AnswerEdit from "./AnswerEdit.js";
import { getErrorMessage } from "../utils/getErrorMessage.js";
import { bindActionCreators } from "redux";
import * as answersActions from "../redux/actions/answersActions.js";
import { connect } from "react-redux";
import {
  getQuestionHasAcceptedAnswer,
  getQuestion,
} from "../redux/reducers/index.js";
import answersReducer from "../redux/reducers/answersReducer.js";

const ManageAnswer = ({
  answersActions,
  answer,
  question,
  questionId,
  questionHasAcceptedAnswer,
  action = "ReadAndEdit",
}) => {
  const [isSaving, setIsSaving] = useState(false);
  const [isDeleting, setIsDeleting] = useState(false);
  const [isAccepting, setIsAccepting] = useState(false);
  const [errors, setErrors] = useState({});
  const [isEditingOrNew, setIsEditingOrNew] = useState(
    action === "New" ? true : false
  );
  const [editedAnswer, setEditedAnswer] = useState(
    answer
      ? answer
      : {
          body: "",
        }
  );

  const onEditToggleHandle = (e) => {
    e.preventDefault();
    if (answer.isOwner) {
      setErrors({});
      setIsEditingOrNew(!isEditingOrNew);
      setEditedAnswer(answer);
    }
  };

  const onSaveNewHandle = async (e) => {
    e.preventDefault();
    if (!isFormValid()) {
      return;
    }
    setIsSaving(true);
    try {
      await answersActions.postAnswer(editedAnswer, questionId);
      setEditedAnswer({ body: "" });
    } catch (error) {
      setErrors({ onSave: getErrorMessage(error) });
    }
    setIsSaving(false);
  };

  const onSaveEditHandle = async (e) => {
    e.preventDefault();
    if (!isFormValid()) {
      return;
    }
    setIsSaving(true);
    try {
      await answersActions.editAnswer(
        editedAnswer,
        questionId,
        editedAnswer.id
      );
      setIsEditingOrNew(false);
    } catch (error) {
      setErrors({ onSave: getErrorMessage(error) });
    }
    setIsSaving(false);
  };

  const onDeleteHandle = async (e) => {
    e.preventDefault();
    if (answer.isOwner) {
      setIsDeleting(true);
      try {
        await answersActions.deleteAnswer(questionId, editedAnswer.id);
      } catch (error) {
        setIsDeleting(false);
        setErrors({ onDelete: getErrorMessage(error) });
      }
    }
  };

  const onAcceptHandle = async (e) => {
    debugger;
    e.preventDefault();
    setIsAccepting(true);
    try {
      await answersActions.acceptAnswer(questionId, answer.id);
    } catch (error) {
      debugger;
      setErrors({ onAccept: getErrorMessage(error) });
    }
    setIsAccepting(false);
  };

  const onInputChange = ({ target }) => {
    setEditedAnswer({ ...editedAnswer, [target.id]: target.value });
  };

  const isFormValid = () => {
    const error = {};
    if (editedAnswer.body.length === 0) {
      error.body = "Answer's body must be at least 100 characters.";
    }
    setErrors(error);
    return Object.keys(error).length === 0;
  };

  return (
    <div>
      {isEditingOrNew ? (
        <AnswerEdit
          answer={editedAnswer}
          onSaveNewAnswer={onSaveNewHandle}
          onSaveEditAnswer={onSaveEditHandle}
          onCancel={onEditToggleHandle}
          action={action}
          isSaving={isSaving}
          onInputChange={onInputChange}
          errors={errors}
        />
      ) : (
        <Answer
          answer={answer}
          onStartEditing={onEditToggleHandle}
          onDelete={onDeleteHandle}
          isDeleting={isDeleting}
          isAcceptable={!questionHasAcceptedAnswer && question.isOwner}
          isAccepting={isAccepting}
          onAccept={onAcceptHandle}
          errors={errors}
        />
      )}
    </div>
  );
};

const mapStateToProps = (state) => {
  return {
    questionHasAcceptedAnswer: getQuestionHasAcceptedAnswer(state),
    question: getQuestion(state),
  };
};

const mapStateToDispatch = (dispatch) => {
  return {
    answersActions: bindActionCreators(answersActions, dispatch),
  };
};

export default connect(mapStateToProps, mapStateToDispatch)(ManageAnswer);
