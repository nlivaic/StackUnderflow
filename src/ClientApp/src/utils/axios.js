import axios from "axios";
import { settings } from "../settings.js";
import { getAccessToken } from "../utils/authService.js";

axios.defaults.baseURL = `${settings.API_URL}`;

axios.interceptors.request.use(
  async (config) => {
    const accessToken = await getAccessToken();
    if (accessToken) {
      config.headers.Authorization = `bearer ${accessToken}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export default axios;
