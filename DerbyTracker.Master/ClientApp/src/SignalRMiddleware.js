import * as signalR from '@aspnet/signalr'
//TODO: Set log level programmatically
const signalrLogLevel = signalR.LogLevel.Information
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/wheelhub")
    .configureLogging(signalrLogLevel)
    .build();

const messageEvent = 'MessageBaseEvent'
const signalrTest = 'SIGNALR_TEST'
const runBout = 'RUN_BOUT'
const assignRole = 'ASSIGN_ROLE'
const removeRole = 'REMOVE_ROLE'
const addToBout = 'ADD_TO_BOUT'

export const actionCreators = {
    signalrTest: () => ({ type: signalrTest }),
    runBout: (boutId) => ({ type: runBout, boutId: boutId }),
    assignRole: (nodeId, role) => ({ type: assignRole, nodeId: nodeId, role: role }),
    removeRole: (nodeId, role) => ({ type: removeRole, nodeId: nodeId, role: role }),
    addToBout: (nodeId, boutId) => ({ type: addToBout, nodeId: nodeId, boutId: boutId })
}

export function signalRInvokeMiddleware(store) {
    return (next) => async (action) => {
        switch (action.type) {
            case messageEvent:
                if (action.messageType === 0) {
                    console.error(action.message)
                } else {
                    console.log(action.message)
                }
                break
            case signalrTest:
                connection.invoke('Test')
                break
            case runBout:
                connection.invoke('RunBout', 'master', action.boutId)
                break
            case assignRole:
                connection.invoke('AssignRole', action.nodeId, action.role)
                break
            case removeRole:
                connection.invoke('RemoveRole', action.nodeId, action.role)
                break
            case addToBout:
                connection.invoke('AddToBout', action.nodeId, action.boutId)
                break
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
        console.log('received: ', data)
        store.dispatch(data)
    })

    connection.start().catch(err => console.error(err.toString())).then(callback);
}