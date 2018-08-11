import uuid from 'uuid'
import { LogLevel } from '@aspnet/signalr'

var settings = null
import { LogLevel } from '@aspnet/signalr'

//debug settings
var newIdOnLoad = false
var IamAScoreboard = false

export function get() {
    if (!settings) {
        settings = JSON.parse(window.localStorage.getItem('settings'))

        if (settings) {
            settings.remoteIp = '192.168.1.10'
            settings.logLevel = LogLevel.Trace
        }

        if (newIdOnLoad) {
            settings.nodeId = uuid.v4()
        }
        if (IamAScoreboard) {
            settings.IamAScoreboard = true;
        }
    }
    if (!settings) {
        settings = {
            nodeId: uuid.v4(),
            remoteIp: '192.168.1.10',
            logLevel: LogLevel.Trace
        }
        save()
    }
    return settings
}

export function save() {
    window.localStorage.setItem('settings', JSON.stringify(settings))
}