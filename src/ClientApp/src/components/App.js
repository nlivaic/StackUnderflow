import React from "react";
import { Route, Switch } from "react-router";
import { Link } from "react-router-dom";
import Profile from "./Auth/Profile.js";
import SigninOidc from "./Auth/SigninOidc.js";
import SignoutOidc from "./Auth/SignoutOidc.js";
import SigninSilentCallback from "./Auth/SigninSilentCallback.js";
import QuestionSummariesList from "./QuestionSummariesList";
import QuestionPage from "./QuestionPage";
import ManageQuestion from "./ManageQuestion";
import { Provider as ReduxProvider } from "react-redux";
import configureStore from "../redux/configureStore.js";
import Search from "./Search.js";
import PageNotFound from "./PageNotFound.js";
import { saveState, loadState } from "../utils/localStorage";
import { getQuestionDraft } from "../redux/reducers/index";
import throttle from "lodash.throttle";

const persistedState = loadState();
const store = configureStore(persistedState);

store.subscribe(
  throttle(() => {
    saveState({
      question: {
        questionDraft: getQuestionDraft(store.getState()),
      },
    });
  }, 1000)
);

const App = () => {
  return (
    <div>
      <ReduxProvider store={store}>
        <Profile />
        <br />
        <Link to="/">Home</Link>
        {/* Persist page size in query string only if a specific page size was chosen previously. */}
        <Search />
        <Link to="/question/ask">Ask Question</Link>
        <Switch>
          <Route exact path="/" component={QuestionSummariesList} />
          <Route exact path="/questions/:questionId" component={QuestionPage} />
          <Route exact path="/question/ask" component={ManageQuestion} />
          <Route exact path="/signin-oidc" component={SigninOidc} />
          <Route
            exact
            path="/signin-silent-oidc"
            component={SigninSilentCallback}
          />
          <Route exact path="/signout-callback-oidc" component={SignoutOidc} />
          <Route exact path="/NotFound" component={PageNotFound} />
          <Route component={PageNotFound} />
        </Switch>
      </ReduxProvider>
    </div>
  );
};

export default App;
