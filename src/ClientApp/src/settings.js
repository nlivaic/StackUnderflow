const commonSettings = {
  LOCAL_STORAGE_KEY: "state",
};

const dev = {
  API_URL: "https://localhost:5001/api",
};

const prod = {
  API_URL: "to_be_defined",
};

export const settings =
  process.env.NODE_ENV === "development"
    ? { ...commonSettings, ...dev }
    : { ...commonSettings, ...prod };
