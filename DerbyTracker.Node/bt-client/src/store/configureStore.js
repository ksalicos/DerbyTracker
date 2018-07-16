import { applyMiddleware, combineReducers, compose, createStore } from 'redux'
import thunk from 'redux-thunk'
import { signalRInvokeMiddleware } from '../SignalRMiddleware'
import * as System from './System'
import * as BoutState from './BoutState'

export default function configureStore(initialState) {
    const reducers = {
        system: System.reducer,
        boutState: BoutState.reducer
    };

    const middleware = [
        thunk,
        signalRInvokeMiddleware
    ];

    // In development, use the browser's Redux dev tools extension if installed
    const enhancers = [];
    const isDevelopment = process.env.NODE_ENV === 'development';
    if (isDevelopment && typeof window !== 'undefined' && window.devToolsExtension) {
        enhancers.push(window.devToolsExtension());
    }

    const rootReducer = combineReducers({
        ...reducers
    });

    return createStore(
        rootReducer,
        initialState,
        compose(applyMiddleware(...middleware), ...enhancers)
    );
}
