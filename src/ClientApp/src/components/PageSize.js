import React from "react";
import { useHistory } from "react-router-dom";
import { PAGE_SIZE } from "../resourceParameters/questionSummaryResourceParameters";

const PageSize = () => {
  const history = useHistory();
  const onPageSizeChange = (e) => {
    e.preventDefault();
    history.push(`?${PAGE_SIZE}=${e.target.value}`);
  };

  return (
    <div>
      <button value="3" onClick={onPageSizeChange}>
        3
      </button>
      <button value="4" onClick={onPageSizeChange}>
        4
      </button>
      <button value="5" onClick={onPageSizeChange}>
        5
      </button>{" "}
      per page
    </div>
  );
};

export default PageSize;
