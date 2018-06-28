import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap/dist/css/bootstrap-theme.css';
import './index.css';
import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import { ConnectedRouter } from 'react-router-redux';
import { createBrowserHistory } from 'history';
import configureStore from './store/configureStore';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import { signalRRegisterCommands } from './SignalRMiddleware'
import superagent from 'superagent'
import { actionCreators } from './store/Bout'

// Create browser history to use in the Redux store
const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const history = createBrowserHistory({ basename: baseUrl });

// Get the application-wide store instance, prepopulating with state from the server where available.
const initialState = window.initialReduxState;
const store = configureStore(history, initialState);

//Initialize SignalR
signalRRegisterCommands(store, () => {
    store.dispatch({ type: 'SIGNALR_CONNECTED' })
    store.dispatch({ type: 'SIGNALR_TEST' })
})

//Load Bout List
superagent
    .get('/api/bout/list')
    .set('Accept', 'application/json')
    .end((e, r) => { store.dispatch(actionCreators.listLoaded(r.body)) })
//TODO: Handle ajax error

const rootElement = document.getElementById('root');

ReactDOM.render(
    <Provider store={store}>
        <ConnectedRouter history={history}>
            <App />
        </ConnectedRouter>
    </Provider>,
    rootElement);

registerServiceWorker();
