import axios from "axios";
import { settings } from "../settings.js";

export async function getAnswers(questionId) {
  try {
    return (
      await axios.get(`${settings.API_URL}/questions/${questionId}/answers`)
    ).data;
  } catch (error) {
    console.error(error);
    throw error;
  }
}

export async function postAnswer(answer, questionId) {
  try {
    return (
      await axios.post(
        `${settings.API_URL}/questions/${questionId}/answers`,
        answer
      )
    ).data;
  } catch (error) {
    console.error(error);
    throw error;
  }
}

export async function editAnswer(answer, questionId, answerId) {
  try {
    await axios.put(
      `${settings.API_URL}/questions/${questionId}/answers/${answerId}`,
      answer
    );
  } catch (error) {
    console.error(error);
    throw error;
  }
}

export async function deleteAnswer(questionId, answerId) {
  try {
    await axios.delete(
      `${settings.API_URL}/questions/${questionId}/answers/${answerId}`
    );
  } catch (error) {
    console.error(error);
    throw error;
  }
}

export async function acceptAnswer(questionId, answerId) {
  try {
    return (
      await axios.post(
        `${settings.API_URL}/questions/${questionId}/answers/${answerId}/acceptAnswer`
      )
    ).data;
  } catch (error) {
    console.error(error);
    throw error;
  }
}
