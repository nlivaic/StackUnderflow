import axios from "../utils/axios";

export async function getComments(parentType, parentIds) {
  try {
    let data;
    if (parentType === "question") {
      data = await getCommentsForQuestion(parentIds);
    } else if (parentType === "answer") {
      data = await getCommentsForAnswers(parentIds);
    }
    return data;
  } catch (error) {
    console.error(error);
    throw error;
  }
}

async function getCommentsForQuestion({ questionId }) {
  return (await axios.get(`questions/${questionId}/comments`)).data;
}

async function getCommentsForAnswers({ questionId, answerIds }) {
  return (
    await axios.get(
      `questions/${questionId}/answers/${answerIds.join(",")}/comments`
    )
  ).data;
}

export async function postComment(comment, parentType, parentIds) {
  try {
    let data;
    if (parentType === "question") {
      data = await postCommentForQuestion(comment, parentIds);
    } else if (parentType === "answer") {
      data = await postCommentForAnswer(comment, parentIds);
    }
    return data;
  } catch (error) {
    console.error(error);
    throw error;
  }
}

async function postCommentForQuestion(comment, { questionId }) {
  return (await axios.post(`questions/${questionId}/comments`, comment)).data;
}

async function postCommentForAnswer(comment, { questionId, answerId }) {
  return (
    await axios.post(
      `questions/${questionId}/answers/${answerId}/comments`,
      comment
    )
  ).data;
}

export async function editComment(comment, parentType, parentIds) {
  try {
    if (parentType === "question") {
      await editCommentForQuestion(comment, parentIds);
    } else if (parentType === "answer") {
      await editCommentForAnswer(comment, parentIds);
    }
  } catch (error) {
    console.error(error);
    throw error;
  }
}

async function editCommentForQuestion(comment, { questionId }) {
  return await axios.put(
    `questions/${questionId}/comments/${comment.id}`,
    comment
  );
}

async function editCommentForAnswer(comment, { questionId, answerId }) {
  return await axios.put(
    `questions/${questionId}/answers/${answerId}/comments/${comment.id}`,
    comment
  );
}

export async function deleteComment(commentId, parentType, parentIds) {
  try {
    if (parentType === "question") {
      await deleteCommentForQuestion(commentId, parentIds);
    } else if (parentType === "answer") {
      await deleteCommentForAnswer(commentId, parentIds);
    }
  } catch (error) {
    console.error(error);
    throw error;
  }
}

async function deleteCommentForQuestion(commentId, { questionId }) {
  return await axios.delete(`questions/${questionId}/comments/${commentId}`);
}

async function deleteCommentForAnswer(commentId, { questionId, answerId }) {
  return await axios.delete(
    `questions/${questionId}/answers/${answerId}/comments/${commentId}`
  );
}
