import * as signalR from '@aspnet/signalr'
//TODO: Set log level programmatically
const signalrLogLevel = signalR.LogLevel.Information
const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:44347/wheelhub")
    .configureLogging(signalrLogLevel)
    .build();

export function signalRInvokeMiddleware(store) {
    return (next) => async (action) => {
        switch (action.type) {
            case "SIGNALR_TEST":
                connection.invoke('Test')
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

    connection.start().catch(err => console.log(err.toString())).then(callback);
}