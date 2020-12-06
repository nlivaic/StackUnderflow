export function getErrorMessage(error) {
  if (error.response.status === 422)
    return error.response.data.errors[
      Object.keys(error.response.data.errors)[0]
    ][0];
  if (error.response.status === 409) {
    return error.response.data[Object.keys(error.response.data)[0]][0];
  }
}
