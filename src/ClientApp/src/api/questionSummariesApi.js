import axios from "../utils/axios";
import { toCamelCase } from "../utils/stringUtils";

export async function getQuestionSummaries(resourceParametersQueryString) {
  try {
    var response = await axios.get(
      `QuestionSummaries${resourceParametersQueryString}`
    );
    let pagination = JSON.parse(response.headers["x-pagination"], toCamelCase);
    return { data: response.data, pagination };
  } catch (error) {
    console.error(error);
    throw error;
  }
}
