import { connection } from '../SignalRMiddleware'
import * as settings from '../Settings'

const updateJammerStatus = 'UPDATE_JAMMER_STATE'
const createPass = 'CREATE_PASS'
const updatePass = 'UPDATE_PASS'

export const actionCreators = {
    updateJammerStatus: (boutId, period, jam, team, status) => ({ type: updateJammerStatus, boutId: boutId, period: period, jam: jam, team: team, status: status }),
    createPass: (b, p, j, t) => ({ type: createPass, boutId: b, period: p, jam: j, team: t }),
    updatePass: (b, p, j, t, pass) => ({ type: updatePass, boutId: b, period: p, jam: j, team: t, pass: pass }),
}

var s = settings.get()
const nodeId = s.nodeId

export function middleware(store) {
    return (next) => async (action) => {
        switch (action.type) {
            case updateJammerStatus:
                connection.invoke('UpdateJammerStatus', nodeId, action.boutId, action.period, action.jam, action.team, action.status)
                break
            case createPass:
                connection.invoke('CreatePass', nodeId, action.boutId, action.period, action.jam, action.team)
                break
            case updatePass:
                connection.invoke('UpdatePass', nodeId, action.boutId, action.period, action.jam, action.team, action.pass)
                break
            default:
                break
        }
        return next(action);
    }
}