import { applyMiddleware, combineReducers, compose, createStore } from 'redux'
import thunk from 'redux-thunk'
import { signalRInvokeMiddleware } from '../SignalRMiddleware'
import * as Counter from './Counter'
import * as System from './System'
import * as Bout from './Bout'
import * as Venue from './Venue'

export default function configureStore(initialState) {
    const reducers = {
        counter: Counter.reducer,
        system: System.reducer,
        bout: Bout.reducer,
        venue: Venue.reducer
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
