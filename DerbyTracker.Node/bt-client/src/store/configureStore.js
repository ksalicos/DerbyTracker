import { applyMiddleware, combineReducers, compose, createStore } from 'redux'
import thunk from 'redux-thunk'
import { signalRMiddleware } from '../SignalRMiddleware'
import * as System from './System'
import * as BoutState from './BoutState'
import * as CurrentJam from './CurrentJam'

export default function configureStore(initialState) {
    const rootReducer = combineReducers({
        system: System.reducer,
        boutState: BoutState.reducer,
        currentJam: CurrentJam.reducer
    });

    const middleware = [
        thunk,
        ...signalRMiddleware
    ];

    // In development, use the browser's Redux dev tools extension if installed
    const enhancers = [];
    const isDevelopment = process.env.NODE_ENV === 'development';
    if (isDevelopment && typeof window !== 'undefined' && window.devToolsExtension) {
        enhancers.push(window.devToolsExtension());
    }

    return createStore(
        rootReducer,
        initialState,
        compose(applyMiddleware(...middleware), ...enhancers)
    );
}
