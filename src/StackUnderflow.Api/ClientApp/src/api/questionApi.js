import axios from "axios";
import * as apiUrl from "../settings";

export async function getQuestion(id) {
  try {
    return (await axios.get(`${apiUrl.API_URL}/questions/${id}`)).data;
  } catch (error) {
    console.error(error);
    throw error;
  }
}

export async function askQuestion(question) {
  try {
    let response = await axios.post(`${apiUrl.API_URL}/questions`, {
      title: question.title,
      body: question.body,
      tagIds: question.tags.map((t) => t.id),
    });
    return response.data;
  } catch (error) {
    console.error(error);
    throw error;
  }
}

export async function editQuestion(id, question) {
  try {
    await axios.put(`${apiUrl.API_URL}/questions/${id}`, {
      title: question.title,
      body: question.body,
      tagIds: question.tags.map((t) => t.id),
    });
  } catch (error) {
    console.error(error);
    throw error;
  }
}

export async function deleteQuestion(id) {
  try {
    await axios.delete(`${apiUrl.API_URL}/questions/${id}`);
  } catch (error) {
    console.error(error);
    throw error;
  }
}
