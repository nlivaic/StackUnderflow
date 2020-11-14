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

export async function saveQuestion(question) {
  try {
    let response = await axios.post(`${apiUrl.API_URL}/questions`, question);
    console.log(response);
    return response;
  } catch (error) {
    console.error(error);
    throw error;
  }
}
