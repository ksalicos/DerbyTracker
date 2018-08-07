import { connection } from '../SignalRMiddleware'
import * as settings from '../Settings'

const buttHitSeat = 'BUTT_HIT_SEAT'
const updateChair = 'UPDATE_CHAIR'
const cancelSit = 'CANCEL_SIT'
const releaseSkater = 'RELASE_SKATER'

export const actionCreators = {
    buttHitSeat: (b, c) => ({ type: buttHitSeat, boutId: b, chair: c }),
    updateChair: (b, c) => ({ type: updateChair, boutId: b, chair: c }),
    cancelSit: (b, id) => ({ type: cancelSit, boutId: b, id: id }),
    releaseSkater: (b, id) => ({ type: releaseSkater, boutId: b, id: id })
}

var s = settings.get()
const nodeId = s.nodeId

export function middleware(store) {
    return (next) => async (action) => {
        switch (action.type) {
            case buttHitSeat:
                connection.invoke('ButtHitSeat', nodeId, action.boutId, action.chair)
                break
            case updateChair:
                connection.invoke('UpdateChair', nodeId, action.boutId, action.chair)
                break
            case cancelSit:
                connection.invoke('CancelSit', nodeId, action.boutId, action.id)
                break
            case releaseSkater:
                connection.invoke('ReleaseSkater', nodeId, action.boutId, action.id)
                break
            default:
                break
        }
        return next(action);
    }
}