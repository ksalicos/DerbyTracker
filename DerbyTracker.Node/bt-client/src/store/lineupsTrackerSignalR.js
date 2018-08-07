import { connection } from '../SignalRMiddleware'
import * as settings from '../Settings'

//Jam Timer
const addSkater = 'ADD_SKATER'
const removeSkater = 'REMOVE_SKATER'
const setSkaterPosition = 'SET_SKATER_POSITION'

export const actionCreators = {
    addSkater: (b, period, jam, team, number) => ({ type: addSkater, boutId: b, period: period, jam: jam, team: team, number: number }),
    removeSkater: (b, period, jam, team, number) => ({ type: removeSkater, boutId: b, period: period, jam: jam, team: team, number: number }),
    setSkaterPosition: (b, period, jam, team, number, position) => ({ type: setSkaterPosition, boutId: b, period: period, jam: jam, team: team, number: number, position: position })
}

var s = settings.get()
const nodeId = s.nodeId

export function middleware(store) {
    return (next) => async (action) => {
        switch (action.type) {
            case addSkater:
                console.log('add')
                connection.invoke('AddSkaterToJam', nodeId, action.boutId, action.period, action.jam, action.team, action.number)
                break
            case removeSkater:
                console.log('removef')
                connection.invoke('RemoveSkaterFromJam', nodeId, action.boutId, action.period, action.jam, action.team, action.number)
                break
            case setSkaterPosition:
                connection.invoke('SetSkaterPosition', nodeId, action.boutId, action.period, action.jam, action.team, action.number, action.position)
                break
            default:
                break
        }
        return next(action);
    }
}