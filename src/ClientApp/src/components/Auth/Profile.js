import React, { useState, useEffect } from "react";
import {
  signinRedirect,
  signoutRedirect,
  signinSilent,
  getUser,
} from "../../utils/authService.js";
import * as profileActions from "../../redux/actions/profileActions.js";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";

const Profile = ({ profile, profileActions }) => {
  const [user, setUser] = useState(null);
  useEffect(() => {
    async function signinSilentAsync() {
      try {
        await signinSilent();
        profileActions.getCurrentUser();
      } catch {} // In case we are not logged into token service.
    }

    async function getUserAsync() {
      if (profile.isLoggedIn && !user) {
        const loggedInUser = await getUser();
        setUser(loggedInUser);
      }
    }
    if (!profile.isLoggedIn) {
      signinSilentAsync();
    }
    getUserAsync();
  }, [profile, user, profileActions]);

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
