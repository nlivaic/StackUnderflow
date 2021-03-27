const commonSettings = {
  LOCAL_STORAGE_KEY: "state",
};

const dev = {
  API_URL: "https://api-local.stack-underflow.com:44395/api",
};

const prod = {
  API_URL: "to_be_defined",
};

export const settings =
  process.env.NODE_ENV === "development"
    ? { ...commonSettings, ...dev }
    : { ...commonSettings, ...prod };
