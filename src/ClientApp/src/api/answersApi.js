import axios from "axios";
import * as apiUrl from "../settings";

export async function getAnswers(questionId) {
  try {
    return (
      await axios.get(`${apiUrl.API_URL}/questions/${questionId}/answers`)
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
        `${apiUrl.API_URL}/questions/${questionId}/answers`,
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
      `${apiUrl.API_URL}/questions/${questionId}/answers/${answerId}`,
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
      `${apiUrl.API_URL}/questions/${questionId}/answers/${answerId}`
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
        `${apiUrl.API_URL}/questions/${questionId}/answers/${answerId}/acceptAnswer`
      )
    ).data;
  } catch (error) {
    console.error(error);
    throw error;
  }
}