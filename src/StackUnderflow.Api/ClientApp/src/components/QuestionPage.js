import React from "react";
import QuestionData from "./QuestionData";
import AnswersList from "./AnswersList";
import ManageAnswer from "./ManageAnswer";
import CommentsList from "./CommentsList.js";
import ManageComment from "./ManageComment.js";
import * as actionTypes from "../utils/actionTypes.js";

const QuestionPage = (props) => {
  const { questionId } = props.match.params;
  return (
    <>
      <div>
        <div>
          <div>Voting</div>
          <QuestionData
            questionId={questionId}
            action={actionTypes.ReadAndEdit}
          />
          <CommentsList parentType="question" parentIds={{ questionId }} />
          <ManageComment
            parentType="question"
            parentIds={{ questionId }}
            action={actionTypes.New}
          />
          <span>
            !!!!!!!!!!!!!!!!!!!!!!!!!!!!!Answers!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
          </span>
          <AnswersList questionId={questionId} />
          <ManageAnswer questionId={questionId} action={actionTypes.New} />
        </div>
      </div>
    </>
  );
};

export default QuestionPage;
