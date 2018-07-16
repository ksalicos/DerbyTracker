import moment from 'moment'
export const initializeBoutState = 'INITIALIZE_BOUT_STATE'
const boutPhaseChanged = 'BOUT_PHASE_CHANGED'
const jamStarted = 'JAM_STARTED'

const initialState = {
    //bouts: {},
    current: null
}

export const actionCreators = {

}

export const reducer = (state, action) => {
    state = state || initialState
    switch (action.type) {
        case initializeBoutState:
            return {
                ...state,
                current: state.current ? state.current : momentify(action.boutState),
                bouts: { ...state.bouts, [action.boutState.boutId]: momentify(action.boutState) }
            }
        case boutPhaseChanged:
            return { ...state, current: { ...state.current, phase: action.newPhase } }
        case jamStarted:
            return {
                ...state,
                current: { ...state.current, phase: 2, clockRunning: true, jamStart: moment() }
            }
        default:
            return state
    }
}

const momentify = boutState => {
    return {
        ...boutState,
        gameTimeElapsed: timespanToSeconds(boutState.gameTimeElapsed),
        jamStart: moment(boutState.jamStart)
    }
}

const timespanToSeconds = ts => {

}

export const phase = [
    'Pre-game', //0
    'Lineup', //1
    'Jam', //2
    'Timeout', //3
    'Halftime', //4
    'UnofficialScore', //5
    'OfficialScore' //6
]