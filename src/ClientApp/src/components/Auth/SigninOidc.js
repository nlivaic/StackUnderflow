import React, { useEffect } from "react";
import { useHistory } from "react-router-dom";
import { bindActionCreators } from "redux";
import { signinRedirectCallback } from "../../utils/authService.js";
import * as profileActions from "../../redux/actions/profileActions";
import { connect } from "react-redux";

const SigninOidc = ({ profileActions }) => {
  const history = useHistory();
  useEffect(() => {
    async function signinAsync() {
      await signinRedirectCallback();
      profileActions.loginSuccess();
      history.push("/");
    }
    signinAsync();
  }, [history, profileActions]);
  return <></>;
};

const mapDispatchToProps = (dispatch) => {
  return {
    profileActions: bindActionCreators(profileActions, dispatch),
  };
};

export default connect(null, mapDispatchToProps)(SigninOidc);
