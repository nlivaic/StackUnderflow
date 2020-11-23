import React, { useEffect, useState } from "react";
import { useHistory, useLocation } from "react-router-dom";
import queryString from "query-string";
import { SORT_BY } from "../resourceParameters/questionSummaryResourceParameters";

const [ASC, DESC] = ["asc", "desc"];

const sortingCriteriaStates = [
  { order: null, selected: false, orderSign: "" },
  { order: ASC, selected: true, orderSign: " ^" },
  { order: DESC, selected: true, orderSign: " ˇ" },
];

const Sorting = ({ resourceSortingCriterias, pageSize }) => {
  const history = useHistory();
  const location = useLocation();
  const [sortingCriteria, setSortingCriteria] = useState([]);

  useEffect(() => {
    let resourceParametersQueryString = queryString.parse(location.search);
    // Pick up sorting criteria from address bar first.
    let resourceParametersInAddress = resourceParametersQueryString[SORT_BY]
      ? resourceParametersQueryString[SORT_BY].split(",").map((criteria) => {
          return {
            value: criteria.split(" ")[0],
            order: criteria.split(" ")[1].toLowerCase(),
          };
        })
      : [];
    setSortingCriteria(
      resourceSortingCriterias.map((criteria) => {
        let criteriaInAddress = resourceParametersInAddress.find(
          (c) => c.value === criteria.value
        );

        let resultSortingCriteria = {
          name: criteria.name,
          displayName: criteria.name,
          value: criteria.value,
        };
        if (!!criteriaInAddress) {
          let sortingCriteriaStatesIndex = sortingCriteriaStates.findIndex(
            (c) => c.order === criteriaInAddress.order
          );
          resultSortingCriteria = {
            ...resultSortingCriteria,
            ...sortingCriteriaStates[sortingCriteriaStatesIndex],
            sortingCriteriaStatesIndex,
            displayName:
              resultSortingCriteria.name +
              sortingCriteriaStates[sortingCriteriaStatesIndex].orderSign,
          };
        } else {
          let sortingCriteriaStatesIndex = 0;
          resultSortingCriteria = {
            ...resultSortingCriteria,
            ...sortingCriteriaStates[sortingCriteriaStatesIndex],
            sortingCriteriaStatesIndex,
          };
        }

        return resultSortingCriteria;
      })
    );
  }, [location, resourceSortingCriterias]);
  const onSortingCriteriaSelect = (e) => {
    e.preventDefault();
    setSortingCriteria(
      sortingCriteria.map((criteria) => {
        if (criteria.value === e.target.value) {
          debugger;
          let index =
            (criteria.sortingCriteriaStatesIndex + 1) %
            sortingCriteriaStates.length;
          return {
            ...criteria,
            ...sortingCriteriaStates[index],
            sortingCriteriaStatesIndex: index,
            displayName: criteria.name + sortingCriteriaStates[index].orderSign,
          };
        }
        return criteria;
      })
    );
  };
  const stringifySelectedSortingCriteria = () => {
    let stringified = sortingCriteria
      .filter((criteria) => criteria.selected)
      .map((criteria) => `${criteria.value} ${criteria.order}`) // This is hardcoded as asc, to toggle asc/desc requires additional effort.
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
            value={sortingCriteria.value}
            key={"available_" + index}
          >
            {sortingCriteria.displayName}
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
            value={sortingCriteria.value}
          >
            {sortingCriteria.displayName}
          </button>
        ))}
      <br />
      <button onClick={applySorting}>Apply Sorting</button>
    </div>
  );
};

export default Sorting;
