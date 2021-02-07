import axios from "../utils/axios";

export async function getUser() {
  try {
    return (await axios.get(`users/current`)).data;
  } catch (error) {
    if (error.response.status === 404) {
      return null;
    }
    console.error(error);
    throw error;
  }
}

export async function createUser(user) {
  try {
    return (await axios.post("users/current", user)).data;
  } catch (error) {
    console.error(error);
    throw error;
  }
}
