import React, { useEffect, useState } from "react";
import { useHistory, useLocation } from "react-router-dom";
import queryString from "query-string";
import {
  SEARCH_QUERY,
  PAGE_SIZE,
} from "../resourceParameters/questionSummaryResourceParameters";

const Search = () => {
  const [searchTerm, setSearchTerm] = useState("");
  const [pageSize, setPageSize] = useState(undefined);
  let location = useLocation();
  let history = useHistory();

  useEffect(() => {
    let parsedQueryString = queryString.parse(location.search);
    if (parsedQueryString[SEARCH_QUERY])
      setSearchTerm(parsedQueryString[SEARCH_QUERY]);
    else setSearchTerm("");
    setPageSize(
      parsedQueryString[PAGE_SIZE] ? parsedQueryString[PAGE_SIZE] : undefined
    );
  }, [location]);

  const onSearchTermChange = (e) => {
    setSearchTerm(e.target.value);
  };

  const onSearchTermSubmit = (e) => {
    e.preventDefault();
    let queryStringParsed = { [SEARCH_QUERY]: searchTerm };
    if (pageSize) {
      queryStringParsed.pageSize = pageSize;
    }
    history.push(`/?${queryString.stringify(queryStringParsed)}`);
  };

  const onSearchTermClear = (e) => {
    e.preventDefault();
    setSearchTerm("");
    let queryStringParsed = {};
    if (pageSize) {
      queryStringParsed.pageSize = pageSize;
    }
    history.push(`?${queryString.stringify(queryStringParsed)}`);
  };

  return (
    <div>
      <input
        type="text"
        onChange={onSearchTermChange}
        placeholder="Search..."
        value={searchTerm}
      />
      <button onClick={onSearchTermSubmit}>Search</button>
      <button onClick={onSearchTermClear}>Clear</button>
    </div>
  );
};

export default Search;
