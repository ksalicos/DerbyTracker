import * as settings from '../Settings'

const signalrConnectedType = 'SIGNALR_CONNECTED'
const changeScreen = 'CHANGE_SCREEN'
const nodeConnected = 'NODE_CONNECTED'

const initialState = {
    screen: 'loading',
    connectionNumber: null,
    roles: [],
    initialization: { complete: false, signalr: false }
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
        case changeScreen:
            newstate = { ...state, screen: action.screen }
            break
        case nodeConnected:
            let s = settings.get()
            if (action.data.nodeId === s.nodeId) {
                newstate = { ...state, connectionNumber: action.data.connectionNumber, roles: action.data.roles }
            }
            break
        default:
            if (!action.type.startsWith('@@redux')) { console.log(action) }
            break
    }

    if (newstate) {
        newstate.initialization.complete = complete(newstate)
        return newstate
    }

    return state;
};

const complete = state => {
    return state.initialization.signalr && state.connectionNumber
}