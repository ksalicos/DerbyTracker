import * as signalR from '@aspnet/signalr'
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/wheelhub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

export function signalRInvokeMiddleware(store: any) {
    return (next: any) => async (action: any) => {
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

export function signalRRegisterCommands(store: any, callback: Function) {
    connection.on('TestAck', data => {
        console.log("Success", data)
    })

    connection.start().catch(err => console.log(err.toString())).then(callback);
}