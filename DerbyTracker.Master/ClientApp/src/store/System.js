const signalrConnectedType = 'SIGNALR_CONNECTED'
const boutListLoaded = 'BOUT_LIST_LOADED'
const venueListLoaded = 'VENUE_LIST_LOADED'
const changeScreen = 'CHANGE_SCREEN'

const venueSelected = 'VENUE_SELECTED'

const initialState = {
    screen: 'loading',
    initialization: { complete: false, signalr: false, boutListLoaded: false, venueListLoaded: false }
};

export const actionCreators = {
    changeScreen: (screen) => ({ type: changeScreen, screen: screen })
    //initialize: (data) => ({ type: initializeType, data: data })
};

const boutScreens = ['bout', 'venue', 'rosters']

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
        case venueListLoaded:
            newstate = {
                ...state, initialization: { ...state.initialization, venueListLoaded: true }
            }
            break
        case changeScreen:
            //TODO: This is getting kludgy - rework
            if (boutScreens.includes(state.screen)) {
                return { ...state, lastBoutScreen: state.screen, screen: action.screen }
            }
            if (action.screen === 'bout' && !boutScreens.includes(state.screen) && state.lastBoutScreen) {
                return { ...state, screen: state.lastBoutScreen }
            }
            return { ...state, screen: action.screen }
        case venueSelected:
            return { ...state, screen: 'bout' }
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
    return state.initialization.signalr && state.initialization.boutListLoaded && state.initialization.venueListLoaded
}