import React from "react";
import { Route } from "react-router";
import QuestionSummariesList from "./QuestionSummariesList";
import QuestionPage from "./QuestionPage";
import { Provider as ReduxProvider } from "react-redux";
import configureStore from "../redux/configureStore.js";
import Search from "./Search.js";

const store = configureStore();

const App = () => {
  return (
    <div>
      {/* Persist page size in query string only if a specific page size was chosen previously. */}
      <Search />
      <ReduxProvider store={store}>
        <Route exact path="/" component={QuestionSummariesList} />
        <Route exact path="/questions/:questionId" component={QuestionPage} />
      </ReduxProvider>
    </div>
  );
};

export default App;
