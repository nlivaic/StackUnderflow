import React, { useState } from "react";
import { useHistory } from "react-router-dom";
import queryString from "query-string";

const Search = ({ pageSize }) => {
  const [searchTerm, setSearchTerm] = useState("");
  const SEARCH_QUERY = "searchQuery";
  let history = useHistory();

  const onSearchTermChange = (e) => {
    setSearchTerm(e.target.value);
  };

  const onSearchTermSubmit = (e) => {
    e.preventDefault();
    let queryStringParsed = { [SEARCH_QUERY]: searchTerm };
    if (pageSize) {
      queryStringParsed.pageSize = pageSize;
    }
    history.push(`?${queryString.stringify(queryStringParsed)}`);
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
