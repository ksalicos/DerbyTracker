import * as signalR from '@aspnet/signalr'
import * as settings from './Settings'
import { nodeConnected } from './store/System'
import { middleware as jamTimer } from './store/jamTimerSignalR'
import { middleware as lineupsTracker } from './store/lineupsTrackerSignalR'
import { middleware as penaltyTracker } from './store/penaltyTrackerSignalR'
import { middleware as scoreKeeper } from './store/scoreKeeperSignalR'

//TODO: Set log level programmatically
const signalrLogLevel = signalR.LogLevel.Information
export const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:44347/wheelhub")
    .configureLogging(signalrLogLevel)
    .build();

var s = settings.get()
const nodeId = s.nodeId

export const signalRMiddleware = [
    signalRInvokeMiddleware,
    jamTimer,
    lineupsTracker,
    penaltyTracker,
    scoreKeeper
]

export function signalRInvokeMiddleware(store) {
    return (next) => async (action) => {
        switch (action.type) {
            case "CONNECT_NODE":
                connection.invoke('ConnectNode', nodeId)
                return;
            case nodeConnected:
                if (action.data.boutId) {
                    console.log(`Joining Bout: ${action.data.boutId}`)
                    connection.invoke('JoinBoutGroup', nodeId, action.data.boutId)
                }
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

    connection.start().catch(err => console.log(err.toString())).then(callback);
}