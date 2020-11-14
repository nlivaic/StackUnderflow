import axios from "axios";
import { API_URL } from "../settings.js";

export async function getComments(parentType, parentId) {
  try {
    const response = await axios.get(
      `${API_URL}/${parentType}/${parentId}/comments`
    );
    return response.data;
  } catch (error) {
    console.error(error);
    throw error;
  }
}

export async function getCommentsForQuestion(questionId) {
  return await getComments("questions", questionId);
}

export async function getCommentsForAnswersAsync(answerId) {
  return await getComments("answers", answerId);
}
