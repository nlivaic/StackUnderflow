import React from "react";
import { Link } from "react-router-dom";
import queryString from "query-string";
import { PAGE_NUMBER } from "../resourceParameters/questionSummaryResourceParameters";

const Paging = ({ resourceParameters, pagingData }) => {
  const { currentPage, totalPages } = pagingData;
  const PREV = "Prev";
  const NEXT = "Next";
  const FIRST_PAGE = 1;
  const LAST_PAGE = totalPages;
  const CURRENT_PAGE = currentPage;
  const THREE_DOTS = "...";
  const CONTIGUOUS = 2;
  const pagingItems = [];
  const contiguousItems = [];
  const createPagingItems = () => {
    if (currentPage > 1) {
      pagingItems.push(PREV);
    }
    pagingItems.push(FIRST_PAGE);
    for (let i = currentPage - CONTIGUOUS; i <= currentPage + CONTIGUOUS; i++) {
      if (i > 1 && i < totalPages) contiguousItems.push(i);
    }
    if (contiguousItems[0] - FIRST_PAGE > 1) pagingItems.push(THREE_DOTS);
    pagingItems.push(...contiguousItems);
    if (LAST_PAGE - contiguousItems[contiguousItems.length - 1] > 1)
      pagingItems.push(THREE_DOTS);
    pagingItems.push(LAST_PAGE);
    if (currentPage < totalPages) {
      pagingItems.push(NEXT);
    }
  };
  const createPagingLinks = () =>
    pagingItems.map((item, index) => {
      switch (item) {
        case PREV:
          return getLink(currentPage - 1, PREV, index);
        case NEXT:
          return getLink(currentPage + 1, NEXT, index);
        case CURRENT_PAGE:
          return getCurrentPage(index);
        case THREE_DOTS:
          return getThreeDots(index);
        default:
          return getLink(item, item, index);
      }
    });

  const getLink = (pageNumber, text, index) => {
    let parsedQueryString = { ...resourceParameters };
    parsedQueryString[PAGE_NUMBER] = pageNumber;
    var qs = queryString.stringify(parsedQueryString);
    return (
      <Link to={`?${qs}`} key={index}>
        {text}
      </Link>
    );
  };
  const getThreeDots = (index) => <span key={index}>...</span>;
  const getCurrentPage = (index) => <span key={index}>{CURRENT_PAGE}</span>;
  createPagingItems();

  return <>{createPagingLinks()}</>;
};

export default Paging;
