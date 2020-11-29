export function getErrorMessage(error) {
  if (error.response.status === 422)
    return error.response.data.errors[
      Object.keys(error.response.data.errors)[0]
    ][0];
}
