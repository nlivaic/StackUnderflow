import axios from "../utils/axios";

export async function upvoteQuestion(questionId) {
  try {
      return (await axios.post(`votes`, {
        targetId: questionId,
        voteTarget: 1,
        voteType: 1
      })).data;
    } catch (error) {
      console.error(error);
      throw error;
    }
  }

export async function downvoteQuestion(questionId) {
  try {
      return (await axios.post(`votes`, {
        targetId: questionId,
        voteTarget: 1,
        voteType: 2
      })).data;
    } catch (error) {
      console.error(error);
      throw error;
    }
  }

export async function revokeVote(voteId) {
  try {
    await axios.delete(`votes/${voteId}`);
  } catch (error) {
    console.error(error);
    throw error;
  }
}
