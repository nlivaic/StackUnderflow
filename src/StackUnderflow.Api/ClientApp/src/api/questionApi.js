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
    let response = await axios.post(`${apiUrl.API_URL}/questions`, question);
    return response.data;
  } catch (error) {
    console.error(error);
    throw error;
  }
}

export async function editQuestion(id, question) {
  try {
    await axios.put(`${apiUrl.API_URL}/questions/${id}`, question);
  } catch (error) {
    console.error(error);
    throw error;
  }
}
