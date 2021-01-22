import axios from "axios";
import { settings } from "../settings.js";
import { getAccessToken } from "../utils/authService.js";

export async function getQuestion(id) {
  try {
    return (await axios.get(`${settings.API_URL}/questions/${id}`)).data;
  } catch (error) {
    console.error(error);
    throw error;
  }
}

export async function askQuestion(question) {
  var accessToken = await getAccessToken();
  try {
    let response = await axios.post(
      `${settings.API_URL}/questions`,
      {
        title: question.title,
        body: question.body,
        tagIds: question.tags.map((t) => t.id),
      },
      { headers: { Authorization: `Bearer ${accessToken}` } }
    );
    return response.data;
  } catch (error) {
    console.error(error);
    throw error;
  }
}

export async function editQuestion(id, question) {
  try {
    await axios.put(`${settings.API_URL}/questions/${id}`, {
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
    await axios.delete(`${settings.API_URL}/questions/${id}`);
  } catch (error) {
    console.error(error);
    throw error;
  }
}
