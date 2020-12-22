import axios from "axios";
import { toCamelCase } from "../utils/stringUtils";
import { settings } from "../settings.js";

export async function getQuestionSummaries(resourceParametersQueryString) {
  try {
    var response = await axios.get(
      `${settings.API_URL}/QuestionSummaries${resourceParametersQueryString}`
    );
    let pagination = JSON.parse(response.headers["x-pagination"], toCamelCase);
    return { data: response.data, pagination };
  } catch (error) {
    console.error(error);
    throw error;
  }
}
