import axios from "../utils/axios";

export async function getQuestion(id) {
  try {
    return (await axios.get(`questions/${id}`)).data;
  } catch (error) {
    console.error(error);
    throw error;
  }
}

export async function askQuestion(question) {
  try {
    let response = await axios.post(`questions`, {
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
    await axios.put(`questions/${id}`, {
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
    await axios.delete(`questions/${id}`);
  } catch (error) {
    console.error(error);
    throw error;
  }
}
