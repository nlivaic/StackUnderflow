import axios from "axios";
import { API_URL } from "../settings.js";

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
  return (await axios.get(`${API_URL}/questions/${questionId}/comments`)).data;
}

async function getCommentsForAnswers({ questionId, answerIds }) {
  return (
    await axios.get(
      `${API_URL}/questions/${questionId}/answers/${answerIds.join(
        ","
      )}/comments`
    )
  ).data;
}
