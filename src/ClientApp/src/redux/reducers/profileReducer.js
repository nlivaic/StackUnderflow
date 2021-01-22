import { LOGIN, LOGOUT } from "../actions/actionTypes.js";

const profileReducer = (state = { isLoggedIn: false }, action) => {
  switch (action.type) {
    case LOGIN:
      return { isLoggedIn: true };
    case LOGOUT:
      return { isLoggedIn: false };
    default:
      return state;
  }
};

export default profileReducer;

// Combined state selector.
export const getProfile = (state) => state;
