import React from "react";
import QuestionData from "./QuestionData";
import AnswersListData from "./AnswersListData";
import ManageAnswer from "./ManageAnswer";
import CommentsListData from "./CommentsListData.js";
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
          <CommentsListData parentType="question" parentIds={{ questionId }} />
          <ManageComment
            parentType="question"
            parentIds={{ questionId }}
            action={actionTypes.New}
          />
          <span>
            !!!!!!!!!!!!!!!!!!!!!!!!!!!!!Answers!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
          </span>
          <AnswersListData questionId={questionId} />
          <ManageAnswer questionId={questionId} action={actionTypes.New} />
        </div>
      </div>
    </>
  );
};

export default QuestionPage;
