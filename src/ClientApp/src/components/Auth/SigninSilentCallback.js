import React, { useEffect } from "react";
import { signinSilentCallback } from "../../utils/authService.js";

const SigninSilentCallback = () => {
  useEffect(() => {
    async function signinSilentCallbackAsync() {
      await signinSilentCallback();
    }
    signinSilentCallbackAsync();
  }, []);
  return <></>;
};

export default SigninSilentCallback;
