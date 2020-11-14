import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import * as questionSummariesApi from "../api/questionSummariesApi";

const QuestionSummariesList = () => {
  const [isLoaded, setIsLoaded] = useState(false);
  const [questionSummariesList, setQuestionSummariesList] = useState([]);

  useEffect(() => {
    async function getQuestionSummaries() {
      const questionSummaries = await questionSummariesApi.getQuestionSummaries();
      setQuestionSummariesList(questionSummaries);
      setIsLoaded(true);
    }
    getQuestionSummaries();
  }, []);
  if (!isLoaded) {
    return (
      <>
        <span>Loading...</span>
      </>
    );
  }
  return (
    <>
      <span>
        <div>
          {questionSummariesList.map((qs) => (
            <div key={qs.id}>
              <div>
                <div>{qs.votesSum} votes</div>
                <div>{qs.Answers} answers</div>
              </div>
              <div>
                <h3>
                  <Link to={`/questions/${qs.id}`}>{qs.title}</Link>
                </h3>
                <div>
                  {qs.tags.map((t) => (
                    <a href="" key={t}>
                      {t}
                    </a>
                  ))}
                </div>
                <div>
                  Asked on {qs.createdOn} by
                  <a href="">{qs.username}</a>
                </div>
              </div>
              <hr />
            </div>
          ))}
        </div>
      </span>
    </>
  );
};

export default QuestionSummariesList;
