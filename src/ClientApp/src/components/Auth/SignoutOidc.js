import React, { useEffect } from "react";
import { useHistory } from "react-router-dom";
import { bindActionCreators } from "redux";
import { signoutRedirectCallback } from "../../utils/authService.js";
import * as profileActions from "../../redux/actions/profileActions";
import { connect } from "react-redux";

const SignoutOidc = ({ profileActions }) => {
  const history = useHistory();
  useEffect(() => {
    async function signoutAsync() {
      await signoutRedirectCallback();
      profileActions.logoutSuccess();
      history.push("/");
    }
    signoutAsync();
  }, [history, profileActions]);
  return <></>;
};

const mapDispatchToProps = (dispatch) => {
  return {
    profileActions: bindActionCreators(profileActions, dispatch),
  };
};

export default connect(null, mapDispatchToProps)(SignoutOidc);
