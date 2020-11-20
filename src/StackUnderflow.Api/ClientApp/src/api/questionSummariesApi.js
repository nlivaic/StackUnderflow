import axios from "axios";
import { toCamelCase } from "../utils/stringUtils";
import * as apiUrl from "../settings";

export async function getQuestionSummaries(resourceParametersQueryString) {
  try {
    debugger;
    var response = await axios.get(
      `${apiUrl.API_URL}/QuestionSummaries${resourceParametersQueryString}`
    );
    let pagination = JSON.parse(response.headers["x-pagination"], toCamelCase);
    return { data: response.data, pagination };
  } catch (error) {
    console.error(error);
    throw error;
  }
}
