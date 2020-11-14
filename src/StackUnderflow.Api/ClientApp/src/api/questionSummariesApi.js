import axios from "axios";
import * as apiUrl from "../settings";

export async function getQuestionSummaries() {
  try {
    return (await axios.get(`${apiUrl.API_URL}/QuestionSummaries`)).data;
  } catch (error) {
    console.error(error);
    throw error;
  }
}
