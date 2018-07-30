import moment from 'moment'
import { setRules, setClocks } from '../clocks'

export const initializeBoutState = 'INITIALIZE_BOUT_STATE'
const updateBoutState = 'UPDATE_BOUT_STATE'
const boutPhaseChanged = 'BOUT_PHASE_CHANGED'
const jamStarted = 'JAM_STARTED'
const jamEnded = 'JAM_ENDED'
const jamUpdated = 'JAM_UPDATED'
const periodEnded = 'PERIOD_ENDED'
const boutEnded = 'BOUT_ENDED'
const penaltyUpdated = 'PENALTY_UPDATED'
//const passUpdated = 'PASS_UPDATED'

const initialState = {
    //bouts: {},
    current: null,
    data: null
}

export const actionCreators = {
}

export const reducer = (state, action) => {
    state = state || initialState
    //This and the below commented code are for when multiple bouts are implemented.
    let convertedState = action.boutState ? momentify(action.boutState) : null
    switch (action.type) {
        case updateBoutState:
            setClocks(action.boutState)
            return { ...state, current: convertedState }
        case initializeBoutState:
            setClocks(action.boutState)
            setRules(action.boutData.ruleSet)
            return {
                data: action.boutData,
                current: convertedState
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
                current: { ...state.current, phase: 1, lineupStart: moment(), jam: state.current.jamnumber + 1 }
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
        case penaltyUpdated:
            let newpenalties
            if (state.current.penalties.some((e) => { return e.id === action.penalty.id })) {
                newpenalties = state.current.penalties.map(e => e.id === action.penalty.id ? action.penalty : e)
            } else {
                newpenalties = [...state.current.penalties, action.penalty]
            }
            return { ...state, current: { ...state.current, penalties: newpenalties } }
        case jamUpdated:
            return {
                ...state, current: {
                    ...state.current, jams: state.current.jams.map(
                        e => e.jamNumber === action.jam.jamNumber && e.period === action.jam.period
                            ? action.jam : e
                    )
                }
            }
        default:
            return state
    }
}

const momentify = boutState => {
    return {
        ...boutState,
        //gameClockElapsed: timespanToSeconds(boutState.gameClockElapsed),
        jamStart: moment(boutState.jamStart)
    }
}

export const phaseList = [
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