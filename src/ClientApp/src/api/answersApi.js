import axios from "../utils/axios";

export async function getAnswers(questionId) {
  try {
    return (await axios.get(`questions/${questionId}/answers`)).data;
  } catch (error) {
    console.error(error);
    throw error;
  }
}

export async function postAnswer(answer, questionId) {
  try {
    return (await axios.post(`questions/${questionId}/answers`, answer)).data;
  } catch (error) {
    console.error(error);
    throw error;
  }
}

export async function editAnswer(answer, questionId, answerId) {
  try {
    await axios.put(`questions/${questionId}/answers/${answerId}`, answer);
  } catch (error) {
    console.error(error);
    throw error;
  }
}

export async function deleteAnswer(questionId, answerId) {
  try {
    await axios.delete(`questions/${questionId}/answers/${answerId}`);
  } catch (error) {
    console.error(error);
    throw error;
  }
}

export async function acceptAnswer(questionId, answerId) {
  try {
    return (
      await axios.post(
        `questions/${questionId}/answers/${answerId}/acceptAnswer`
      )
    ).data;
  } catch (error) {
    console.error(error);
    throw error;
  }
}
