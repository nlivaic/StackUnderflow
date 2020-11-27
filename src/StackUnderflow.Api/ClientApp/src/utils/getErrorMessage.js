export function getErrorMessage(error) {
  if (error.response.status === 422) return error.response.data.errors[""][0];
}
