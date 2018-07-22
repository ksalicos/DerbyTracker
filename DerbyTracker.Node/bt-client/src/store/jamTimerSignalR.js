import { connection } from '../SignalRMiddleware'
import * as settings from '../Settings'

//Jam Timer
const exitPregame = 'EXIT_PREGAME'
const startJam = 'START_JAM'
const stopJam = 'STOP_JAM'
const startTimeout = 'START_TIMEOUT'
const stopTimeout = 'STOP_TIMEOUT'
const setTimeoutType = 'CHANGE_TIMEOUT_TYPE'
const setLoseReview = 'SET_LOSE_OFFICIAL_REVIEW'

export const actionCreators = {
    exitPregame: (b) => ({ type: exitPregame, boutId: b }),
    startJam: (b) => ({ type: startJam, boutId: b }),
    stopJam: (b) => ({ type: stopJam, boutId: b }),
    startTimeout: (b) => ({ type: startTimeout, boutId: b }),
    stopTimeout: (b) => ({ type: stopTimeout, boutId: b }),
    setTimeoutType: (b, t) => ({ type: setTimeoutType, boutId: b, timeoutType: t }),
    setLoseReview: (b, l) => ({ type: setLoseReview, boutId: b, loseReview: l })
}

var s = settings.get()
const nodeId = s.nodeId

export function middleware(store) {
    return (next) => async (action) => {
        switch (action.type) {
            case exitPregame:
                connection.invoke('ExitPregame', nodeId, action.boutId)
                break
            case startJam:
                connection.invoke('StartJam', nodeId, action.boutId)
                break
            case stopJam:
                connection.invoke('StopJam', nodeId, action.boutId)
                break
            case startTimeout:
                connection.invoke('StartTimeout', nodeId, action.boutId)
                break
            case stopTimeout:
                connection.invoke('StopTimeout', nodeId, action.boutId)
                break
            case setTimeoutType:
                connection.invoke('SetTimeoutType', nodeId, action.boutId, action.timeoutType)
                break
            case setLoseReview:
                connection.invoke('SetLoseOfficialReview', nodeId, action.boutId, action.loseReview)
                break
            default:
                break
        }
        return next(action);
    }
}