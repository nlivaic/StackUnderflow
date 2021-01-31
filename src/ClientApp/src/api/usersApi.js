import axios from "axios";
import { settings } from "../settings.js";
import { getAccessToken } from "../utils/authService.js";

export async function getUser() {
  var accessToken = await getAccessToken();
  try {
    return (
      await axios.get(`${settings.API_URL}/users/current`, {
        headers: { Authorization: `Bearer ${accessToken}` },
      })
    ).data;
  } catch (error) {
    console.error(error);
    throw error;
  }
}
