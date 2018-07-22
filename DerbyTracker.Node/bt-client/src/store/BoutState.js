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
            let convertedState = momentify(action.boutState)
            return {
                current: convertedState,
                //current: state.current ? state.current : convertedState,
                //bouts: { ...state.bouts, [action.boutState.boutId]: convertedState }
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
        gameClockElapsed: timespanToSeconds(boutState.gameClockElapsed),
        jamStart: moment(boutState.jamStart)
    }
}

const timespanToSeconds = ts => {
    //gameClockElapsed:"00:00:33.0023251"
    var arr = ts.split(':')
    if (arr.length !== 3) {
        console.log('Invalid timestamp recieved')
        return 0
    }
    var sec = arr[2].split('.')
    var ms = Math.floor(sec[1] / 1000)

    var time = ms + sec[0] * 1000 + arr[1] * 60000 + arr[0] * 36000000
    return time
}

export const phase = [
    'Pre-game', //0
    'Lineup', //1
    'Jam', //2
    'Timeout', //3
    'Halftime', //4
    'UnofficialFinal', //5
    'Final' //6
]

export const timeoutType = [
    'Official',//0
    'LeftTeam',//1
    'RightTeam',//2
    'LeftReview',//3
    'RightReview'//4
]