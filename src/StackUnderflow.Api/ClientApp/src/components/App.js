import React from "react";
import { Route } from "react-router";
import { Link } from "react-router-dom";
import QuestionSummariesList from "./QuestionSummariesList";
import QuestionPage from "./QuestionPage";
import ManageQuestionPage from "./ManageQuestionPage";
import { Provider as ReduxProvider } from "react-redux";
import configureStore from "../redux/configureStore.js";
import Search from "./Search.js";

const store = configureStore();

const App = () => {
  return (
    <div>
      <Link to="/">Home</Link>
      {/* Persist page size in query string only if a specific page size was chosen previously. */}
      <Search />
      <Link to="/question/ask">Ask Question</Link>
      <ReduxProvider store={store}>
        <Route exact path="/" component={QuestionSummariesList} />
        <Route exact path="/questions/:questionId" component={QuestionPage} />
        <Route exact path="/question/ask" component={ManageQuestionPage} />
      </ReduxProvider>
    </div>
  );
};

export default App;
