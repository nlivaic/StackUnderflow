import React, { useEffect, useState } from "react";
import { Link, useLocation } from "react-router-dom";
import * as questionSummariesApi from "../api/questionSummariesApi";
import queryString from "query-string";
import Paging from "./Paging.js";
import Sorting from "./Sorting.js";

const QuestionSummariesList = (props) => {
  const [isLoaded, setIsLoaded] = useState(false);
  const [questionSummariesList, setQuestionSummariesList] = useState([]);
  const [resourceParameters, setResourceParameters] = useState({
    pageSize: "",
    pageNumber: "",
    tags: "",
    users: "",
    searchQuery: "",
    sortBy: "",
  });
  const [responsePagingData, setResponsePagingData] = useState({
    currentPage: "",
    totalPages: "",
    totaItems: "",
  });
  const location = useLocation();
  useEffect(() => {
    async function getQuestionSummaries() {
      const questionSummariesResponse = await questionSummariesApi.getQuestionSummaries(
        location.search
      );
      setQuestionSummariesList(questionSummariesResponse.data);
      setResourceParameters(queryString.parse(location.search));
      setResponsePagingData(questionSummariesResponse.pagination);
      setIsLoaded(true);
    }
    getQuestionSummaries();
  }, [location]);
  if (!isLoaded) {
    return (
      <>
        <span>Loading...</span>
      </>
    );
  }
  return (
    <div>
      <Sorting
        resourceSortingCriterias={[
          "Username",
          "HasAcceptedAnswer",
          "CreatedOn",
        ]}
      ></Sorting>
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
      <Paging
        resourceParameters={resourceParameters}
        pagingData={responsePagingData}
      />
    </div>
  );
};

export default QuestionSummariesList;
