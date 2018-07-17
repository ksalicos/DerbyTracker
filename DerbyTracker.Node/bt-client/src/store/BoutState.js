import moment from 'moment'
export const initializeBoutState = 'INITIALIZE_BOUT_STATE'
const boutPhaseChanged = 'BOUT_PHASE_CHANGED'
const jamStarted = 'JAM_STARTED'
const jamEnded = 'JAM_ENDED'
const periodEnded = 'PERIOD_ENDED'
const boutEnded = 'BOUT_ENDED'

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
            let lastClock = state.current.clockRunning ? state.current.lastClockStart : moment()
            return {
                ...state,
                current: { ...state.current, phase: 2, clockRunning: true, jamStart: moment(), lastClockStart: lastClock }
            }
        case jamEnded:
            return {
                ...state,
                current: { ...state.current, phase: 1, lineupStart: moment(), jam: state.current.jam + 1 }
            }
        case periodEnded:
            return {
                ...state,
                current: { ...state.current, phase: 4, jam: 1, period: state.period + 1, lastClockStart: moment() }
            }
        case boutEnded:
            return {
                ...state,
                current: { ...state.current, phase: 5 }
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
    return 0
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