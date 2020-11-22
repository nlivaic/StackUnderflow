import React, { useEffect, useState } from "react";
import { useHistory, useLocation } from "react-router-dom";
import queryString from "query-string";

const Sorting = ({ resourceSortingCriterias, pageSize }) => {
  const SORT_BY = "sortBy";
  const history = useHistory();
  const location = useLocation();
  const [sortingCriteria, setSortingCriteria] = useState([]);

  useEffect(() => {
    let resourceParametersQueryString = queryString.parse(location.search);
    setSortingCriteria(
      resourceSortingCriterias.map((criteria) => {
        let resourceParametersInAddress = resourceParametersQueryString[SORT_BY]
          ? resourceParametersQueryString[SORT_BY].split(",").map(
              (criteria) => criteria.split(" ")[0]
            )
          : [];

        return {
          name: criteria,
          selected: resourceParametersInAddress.includes(criteria),
        };
      })
    );
  }, [location, resourceSortingCriterias]);
  const onSortingCriteriaSelect = (e) => {
    e.preventDefault();
    setSortingCriteria(
      sortingCriteria.map((criteria) => {
        if (criteria.name === e.target.value)
          criteria.selected = !criteria.selected;
        return criteria;
      })
    );
  };
  const stringifySelectedSortingCriteria = () => {
    let stringified = sortingCriteria
      .filter((criteria) => criteria.selected)
      .map((criteria) => criteria.name + " asc") // This is hardcoded as asc, to toggle asc/desc requires additional effort.
      .join(",");
    return stringified;
  };
  const applySorting = (e) => {
    e.preventDefault();
    let stringified = stringifySelectedSortingCriteria();
    let queryStringParsed =
      stringified.length > 0 ? { [SORT_BY]: stringified } : {};
    if (pageSize) {
      queryStringParsed.pageSize = pageSize;
    }
    history.push(`?${queryString.stringify(queryStringParsed)}`);
  };
  return (
    <div>
      Available sorting criteria:
      {sortingCriteria
        .filter((sortingCriteria) => sortingCriteria.selected === false)
        .map((sortingCriteria, index) => (
          <button
            onClick={onSortingCriteriaSelect}
            value={sortingCriteria.name}
            key={"available_" + index}
          >
            {sortingCriteria.name}
          </button>
        ))}
      <br />
      Selected sorting criteria:
      {sortingCriteria
        .filter((sortingCriteria) => sortingCriteria.selected === true)
        .map((sortingCriteria, index) => (
          <button
            onClick={onSortingCriteriaSelect}
            key={"selected_" + index}
            value={sortingCriteria.name}
          >
            {sortingCriteria.name}
          </button>
        ))}
      <br />
      <button onClick={applySorting}>Apply Sorting</button>
    </div>
  );
};

export default Sorting;
