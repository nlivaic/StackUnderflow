export function getErrorMessage(error) {
  if (error.response.status === 422 || error.response.status === 409) {
    let errors = error.response.data["Errors"] || error.response.data["errors"];
    return errors[
      Object.keys(errors)[0]
    ][0];
  }
}
