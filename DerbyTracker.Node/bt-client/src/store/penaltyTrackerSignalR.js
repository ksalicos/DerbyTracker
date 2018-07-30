import { connection } from '../SignalRMiddleware'
import * as settings from '../Settings'

const createPenalty = 'CREATE_PENALTY'
const updatePenalty = 'UPDATE_PENALTY'

export const actionCreators = {
    createPenalty: (boutId, period, jam, team) => ({ type: createPenalty, boutId: boutId, period: period, jam: jam, team: team }),
    updatePenalty: (boutId, penalty) => ({ type: updatePenalty, boutId: boutId, penalty: penalty })
}

var s = settings.get()
const nodeId = s.nodeId

export function middleware(store) {
    return (next) => async (action) => {
        switch (action.type) {
            case createPenalty:
                connection.invoke('CreatePenalty', nodeId, action.boutId, action.period, action.jam, action.team)
                break
            case updatePenalty:
                connection.invoke('UpdatePenalty', nodeId, action.boutId, action.penalty)
                break
            default:
                break
        }
        return next(action);
    }
}