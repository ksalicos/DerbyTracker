import * as signalR from '@aspnet/signalr'
import * as settings from './Settings'
import { middleware as jamTimer } from './store/jamTimerSignalR'
import { middleware as lineupsTracker } from './store/lineupsTrackerSignalR'
import { middleware as penaltyTracker } from './store/penaltyTrackerSignalR'
import { middleware as scoreKeeper } from './store/scoreKeeperSignalR'
import { middleware as penaltyBox } from './store/penaltyBoxTimerSignalR'
import { initializeBoutState } from './store/BoutState'

var s = settings.get()
const nodeId = s.nodeId
const remoteIp = `http://${s.remoteIp}/wheelhub`

const signalrLogLevel = s.logLevel
export const connection = new signalR.HubConnectionBuilder()
    .withUrl(remoteIp)
    .configureLogging(signalrLogLevel)
    .build();

export const signalRMiddleware = [
    signalRInvokeMiddleware,
    jamTimer,
    lineupsTracker,
    penaltyTracker,
    scoreKeeper,
    penaltyBox
]

export function signalRInvokeMiddleware(store) {
    return (next) => async (action) => {
        switch (action.type) {
            case "CONNECT_NODE":
                connection.invoke('ConnectNode', nodeId)
                return;
            case initializeBoutState:
                connection.invoke('JoinBoutGroup', nodeId, action.boutState.boutId)
                break;
            default:
                break
        }
        return next(action);
    }
}

export function signalRRegisterCommands(store, callback) {
    connection.on('TestAck', data => {
        console.log("SignalR Test: Success")
    })
    connection.on('dispatch', data => {
        console.log('signalr event:', data)
        store.dispatch(data)
    })

    connection.start().catch(err => console.log('SignalR Startup Error', err.toString())).then(callback);
}