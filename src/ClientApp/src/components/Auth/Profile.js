import React, { useState, useEffect } from "react";
import {
  signinRedirect,
  signoutRedirect,
  getUser,
} from "../../utils/authService.js";
import * as profileActions from "../../redux/actions/profileActions.js";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";

const Profile = ({ profile, profileActions }) => {
  const [user, setUser] = useState(null);
  useEffect(() => {
    async function signinAsync() {
      const loggedInUser = await getUser();
      if (loggedInUser && !user) {
        setUser(loggedInUser);
        profileActions.getCurrentUser();
      }
    }
    signinAsync();
    // eslint-disable-next-line
  }, [profile]);

  return (
    <>
      {user ? (
        <>
          {`Hi, ${user.profile.nickname} (${profile.roles.join(", ")}) `}
          <button
            onClick={async (e) => {
              e.preventDefault();
              await signoutRedirect();
            }}
          >
            Sign Out
          </button>
        </>
      ) : (
        <button
          onClick={async (e) => {
            e.preventDefault();
            await signinRedirect();
          }}
        >
          Sign In
        </button>
      )}
    </>
  );
};

const mapDispatchToProps = (dispatch) => {
  return {
    profileActions: bindActionCreators(profileActions, dispatch),
  };
};

const mapStateToProps = (state) => {
  return {
    profile: state.profile, // Reading straight from state because of a weird loop happening.
  };
};

export default connect(mapStateToProps, mapDispatchToProps)(Profile);
