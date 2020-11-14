import React, { Component } from "react";
import { Route } from "react-router";
import QuestionSummariesList from "./QuestionSummariesList";
import QuestionPage from "./QuestionPage";
import { Provider as ReduxProvider } from "react-redux";
import configureStore from "../redux/configureStore.js";

const store = configureStore();

export default class App extends Component {
  render() {
    return (
      <div>
        <ReduxProvider store={store}>
          <Route exact path="/" component={QuestionSummariesList} />
          <Route exact path="/questions/:questionId" component={QuestionPage} />
        </ReduxProvider>
      </div>
    );
  }
}
