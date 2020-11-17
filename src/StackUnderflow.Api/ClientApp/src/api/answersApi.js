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
    let response = await axios.post(
      `${apiUrl.API_URL}/questions/${questionId}/answers`,
      answer
    );
    console.log(response);
    return response;
  } catch (error) {
    console.error(error);
    throw error;
  }
}
