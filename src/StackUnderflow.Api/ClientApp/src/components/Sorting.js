import React, { useState } from "react";
import { useHistory } from "react-router-dom";

const Sorting = ({ resourceSortingCriterias }) => {
  const SORT_BY = "sortBy";
  const history = useHistory();
  const [sortingCriteria, setSortingCriteria] = useState(
    resourceSortingCriterias.map((criteria) => {
      return { name: criteria, selected: false };
    })
  );
  const onSortingCriteriaSelect = (e) => {
    setSortingCriteria(
      sortingCriteria.map((criteria) => {
        if (criteria.name === e.target.innerText)
          criteria.selected = !criteria.selected;
        return criteria;
      })
    );
  };
  const stringifySelectedSortingCriteria = () => {
    let stringified = sortingCriteria
      .filter((criteria) => criteria.selected)
      .map((criteria) => criteria.name + " asc")
      .join(",");
    return stringified;
  };
  const applySorting = (e) => {
    debugger;
    e.preventDefault();
    let stringified = stringifySelectedSortingCriteria();
    if (stringified.length > 0) {
      history.push(`?${SORT_BY}=${stringified}`);
    } else {
      history.push("");
    }
  };
  return (
    <div>
      Available sorting criteria:
      {sortingCriteria
        .filter((sortingCriteria) => sortingCriteria.selected === false)
        .map((sortingCriteria, index) => (
          <span onClick={onSortingCriteriaSelect} key={"available_" + index}>
            {sortingCriteria.name}
          </span>
        ))}
      <br />
      Selected sorting criteria:
      {sortingCriteria
        .filter((sortingCriteria) => sortingCriteria.selected === true)
        .map((sortingCriteria, index) => (
          <span onClick={onSortingCriteriaSelect} key={"selected_" + index}>
            {sortingCriteria.name}
          </span>
        ))}
      <br />
      <button onClick={applySorting}>Apply Sorting</button>
    </div>
  );
};

export default Sorting;
