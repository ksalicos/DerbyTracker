import { applyMiddleware, combineReducers, compose, createStore } from 'redux'
import thunk from 'redux-thunk'
import { signalRInvokeMiddleware } from '../SignalRMiddleware'
import * as Counter from './Counter'
import * as WeatherForecasts from './WeatherForecasts'
import * as System from './System'
import * as Bout from './Bout'

export default function configureStore(initialState) {
    const reducers = {
        counter: Counter.reducer,
        weatherForecasts: WeatherForecasts.reducer,
        system: System.reducer,
        bout: Bout.reducer
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
