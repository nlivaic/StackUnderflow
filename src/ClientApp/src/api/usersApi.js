import axios from "../utils/axios";

export async function getUser() {
  try {
    return (await axios.get(`users/current`)).data;
  } catch (error) {
    console.error(error);
    throw error;
  }
}
