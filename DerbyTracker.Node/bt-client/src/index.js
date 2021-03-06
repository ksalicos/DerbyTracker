import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap/dist/css/bootstrap-theme.css';
import 'normalize.css'
import './index.css';
import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import configureStore from './store/configureStore';
import App from './App';
//import registerServiceWorker from './registerServiceWorker';
import { signalRRegisterCommands } from './SignalRMiddleware'
import * as settings from './Settings'
import * as logger from './logger'
import * as aliases from './aliases'

setTimeout(() => { aliases.fullScreen.goFullScreen() }, 100)

let s = settings.get()
logger.log('Node Initializing: ' + s.nodeId)

// Get the application-wide store instance, prepopulating with state from the server where available.
const initialState = window.initialReduxState
const store = configureStore(initialState)
//Initialize SignalR
signalRRegisterCommands(store, () => {
    store.dispatch({ type: 'SIGNALR_CONNECTED' })
    store.dispatch({ type: 'CONNECT_NODE' })
})

const rootElement = document.getElementById('root');

if (s.IamAScoreboard) {
    store.dispatch({ type: 'CHANGE_SCREEN', screen: 'scoreboard' })
}

ReactDOM.render(
    <Provider store={store}>
        <App />
    </Provider>,
    rootElement);
//registerServiceWorker();