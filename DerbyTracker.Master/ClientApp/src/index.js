import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap/dist/css/bootstrap-theme.css';
import './index.css';
import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import configureStore from './store/configureStore';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import { signalRRegisterCommands } from './SignalRMiddleware'
import superagent from 'superagent'
import noCache from 'superagent-no-cache'
import { actionCreators as bout } from './store/Bout'
import { actionCreators as venue } from './store/Venue'

// Get the application-wide store instance, prepopulating with state from the server where available.
const initialState = window.initialReduxState
const store = configureStore(initialState)

//Initialize SignalR
signalRRegisterCommands(store, () => {
    store.dispatch({ type: 'SIGNALR_CONNECTED' })
    store.dispatch({ type: 'SIGNALR_TEST' })
})

//Load Bout List
superagent
    .get('/api/bout/list')
    .use(noCache)
    .set('Accept', 'application/json')
    .end((e, r) => { store.dispatch(bout.listLoaded(r.body)) })
//Load Venue List
superagent
    .get('/api/venue/list')
    .use(noCache)
    .set('Accept', 'application/json')
    .end((e, r) => { store.dispatch(venue.listLoaded(r.body)) })
//TODO: Handle ajax errors

const rootElement = document.getElementById('root');

ReactDOM.render(
    <Provider store={store}>
        <App />
    </Provider>,
    rootElement);
registerServiceWorker();