import { connection } from '../SignalRMiddleware'
import * as settings from '../Settings'

//Jam Timer
const addSkater = 'ADD_SKATER'


export const actionCreators = {
    addSkater: (b, period, jam, team, number) => ({ type: addSkater, boutId: b, period: period, jam: jam, team: team, number: number }),
}

var s = settings.get()
const nodeId = s.nodeId

export function middleware(store) {
    return (next) => async (action) => {
        switch (action.type) {
            case addSkater:
                console.log(action)
                connection.invoke('AddSkaterToJam', nodeId, action.boutId, action.period, action.jam, action.team, action.number)
                break

            default:
                break
        }
        return next(action);
    }
}