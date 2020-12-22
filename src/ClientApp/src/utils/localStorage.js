import { settings } from "../settings";

export const loadState = () => {
  try {
    const serializedState = localStorage.getItem(settings.LOCAL_STORAGE_KEY);
    if (serializedState === null) {
      return undefined;
    }
    return JSON.parse(serializedState);
  } catch {
    return undefined;
  }
};

export const saveState = (state) => {
  try {
    const serializedState = JSON.stringify(state);
    localStorage.setItem(settings.LOCAL_STORAGE_KEY, serializedState);
  } catch {
    console.error(`Error logging state: ${state}`);
  }
};
