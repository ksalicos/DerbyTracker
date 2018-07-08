const signalrConnectedType = 'SIGNALR_CONNECTED'
const boutListLoaded = 'BOUT_LIST_LOADED'
const changeScreen = 'CHANGE_SCREEN'

const initialState = {
    screen: 'loading',
    initialization: { complete: false, signalr: false, boutListLoaded: false }
};

export const actionCreators = {
    changeScreen: (screen) => ({ type: changeScreen, screen: screen })
    //initialize: (data) => ({ type: initializeType, data: data })
};

export const reducer = (state, action) => {
    state = state || initialState
    let newstate
    switch (action.type) {
        case signalrConnectedType:
            newstate = {
                ...state, initialization: { ...state.initialization, signalr: true }
            }
            break
        case boutListLoaded:
            newstate = {
                ...state, initialization: { ...state.initialization, boutListLoaded: true }
            }
            break
        case changeScreen:
            return { ...state, screen: action.screen }
        default:
            break
    }

    if (newstate) {
        newstate.initialization.complete = complete(newstate)
        return newstate
    }

    return state;
};

const complete = state => {
    return state.initialization.signalr && state.initialization.boutListLoaded
}