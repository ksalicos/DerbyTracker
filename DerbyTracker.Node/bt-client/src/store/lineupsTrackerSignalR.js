import { connection } from '../SignalRMiddleware'
import * as settings from '../Settings'

//Jam Timer
const addSkater = 'ADD_SKATER'


export const actionCreators = {
    addSkater: (jam, team, number, b) => ({ type: addSkater, boutId: b, jam, team, number }),
}

var s = settings.get()
const nodeId = s.nodeId

export function middleware(store) {
    return (next) => async (action) => {
        switch (action.type) {
            case addSkater:
                connection.invoke('AddSkater', nodeId, action.boutId)
                break

            default:
                break
        }
        return next(action);
    }
}