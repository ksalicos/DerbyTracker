import * as settings from '../Settings'
import { initializeBoutState } from './BoutState'
const signalrConnectedType = 'SIGNALR_CONNECTED'
const changeScreen = 'CHANGE_SCREEN'
const nodeConnected = 'NODE_CONNECTED'
const rolesUpdated = 'NODE_ROLES_UPDATED'

const initialState = {
    screen: 'loading',
    connectionNumber: null,
    boutId: null,
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
                newstate = {
                    ...state,
                    connectionNumber: action.data.connectionNumber,
                    roles: action.data.roles,
                    boutId: action.data.boutId
                }
            }
            break
        case rolesUpdated:
            return { ...state, roles: action.newRoles }
        case initializeBoutState:
            if (!state.boutId) {
                return { ...state, boutId: action.boutState.boutId }
            }
            break;
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
    return state.initialization.signalr && state.connectionNumber && state.boutId
}