import * as signalR from '@aspnet/signalr'
import * as settings from './Settings'

//TODO: Set log level programmatically
const signalrLogLevel = signalR.LogLevel.Information
const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:44347/wheelhub")
    .configureLogging(signalrLogLevel)
    .build();

var s = settings.get()
const nodeId = s.nodeId

export function signalRInvokeMiddleware(store) {
    return (next) => async (action) => {
        switch (action.type) {
            case "CONNECT_NODE":
                connection.invoke('ConnectNode', nodeId)
                break;
            default:
                break;
        }

        return next(action);
    }
}

export function signalRRegisterCommands(store, callback) {
    connection.on('TestAck', data => {
        console.log("SignalR Test: Success")
    })
    connection.on('dispatch', data => {
        store.dispatch(data)
    })

    connection.start().catch(err => console.log(err.toString())).then(callback);
}