const signalrConnectedType = 'SIGNALR_CONNECTED'
const boutListLoaded = 'BOUT_LIST_LOADED'

const initialState = {
    initialization: { complete: false, signalr: false, boutListLoaded: false }
};

export const actionCreators = {
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
    console.log(state)
    return state.initialization.signalr && state.initialization.boutListLoaded
}