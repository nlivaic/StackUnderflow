import React, { useEffect, useState } from "react";
import { Link, useLocation } from "react-router-dom";
import * as questionSummariesApi from "../api/questionSummariesApi";
import queryString from "query-string";
import Paging from "./Paging.js";
import Sorting from "./Sorting.js";
import PageSize from "./PageSize.js";

const QuestionSummariesList = () => {
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
    currentPageSize: "",
  });
  const questionSummariesWithSortingAndPaging = () => (
    <div>
      {/* Persist page size in query string only if a specific page size was chosen previously. */}
      <Sorting
        resourceSortingCriterias={[
          { name: "Username", value: "Username" },
          { name: "Has Accepted Answer", value: "HasAcceptedAnswer" },
          { name: "Created On", value: "CreatedOn" },
        ]}
        pageSize={
          resourceParameters.pageSize
            ? responsePagingData.currentPageSize
            : undefined
        }
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
              Tags:{" "}
              {qs.tags.map(
                (t) => (
                  // <a href="" key={t}>
                  <span key={t}>{t} </span>
                )
                // </a>
              )}
            </div>
            <div>
              Asked on {qs.createdOn} by {qs.username}
            </div>
          </div>
          <hr />
        </div>
      ))}
      <Paging
        resourceParameters={resourceParameters}
        pagingData={responsePagingData}
      />
      <PageSize />
    </div>
  );
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
      {questionSummariesList.length === 0 ? (
        <span>Nothing found.</span>
      ) : (
        questionSummariesWithSortingAndPaging()
      )}
    </div>
  );
};

export default QuestionSummariesList;
