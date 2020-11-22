import React from "react";
import { Link } from "react-router-dom";
import queryString from "query-string";

const Paging = ({ resourceParameters, pagingData, url }) => {
  const { currentPage, totalPages } = pagingData;
  const PREV = "Prev";
  const NEXT = "Next";
  const FIRST_PAGE = 1;
  const LAST_PAGE = totalPages;
  const CURRENT_PAGE = currentPage;
  const THREE_DOTS = "...";
  const CONTIGUOUS = 2;
  const pagingItems = [];
  const createPagingItems = () => {
    if (currentPage > 1) pagingItems.push(PREV);
    if (currentPage !== 1) pagingItems.push(FIRST_PAGE);
    if (currentPage - CONTIGUOUS > 2) pagingItems.push(THREE_DOTS);
    if (currentPage - CONTIGUOUS > 1)
      pagingItems.push(currentPage - CONTIGUOUS);
    if (currentPage - CONTIGUOUS > 0)
      pagingItems.push(currentPage - CONTIGUOUS + 1);
    pagingItems.push(currentPage);
    if (currentPage + CONTIGUOUS < totalPages)
      pagingItems.push(currentPage + CONTIGUOUS - 1);
    if (currentPage + CONTIGUOUS < totalPages)
      pagingItems.push(currentPage + CONTIGUOUS);
    if (currentPage + CONTIGUOUS + 1 < totalPages) pagingItems.push(THREE_DOTS);
    if (currentPage !== totalPages) pagingItems.push(LAST_PAGE);
    if (currentPage < totalPages) pagingItems.push(NEXT);
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
    var qs = queryString.stringify({ ...resourceParameters, pageNumber });
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
