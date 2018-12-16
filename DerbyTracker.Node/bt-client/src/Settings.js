import uuid from 'uuid'
import { LogLevel } from '@aspnet/signalr'

var settings = null

//debug settings - set true to override settings file
var newIdOnLoad = false
var IamAScoreboard = false
var forceNewSettings = false

var defaults = {
    remoteIp: '192.168.86.36:5000',
    logLevel: LogLevel.Trace,
    nodeId: uuid.v4(),
    IamAScoreboard: false,
    newIdOnLoad: false
}

//TODO: This needs to be user editable somehow, so that they can set their own IP
export function get() {
    if (!settings) {
        try {
            let loaded = JSON.parse(window.localStorage.getItem('settings'))
            settings = { ...defaults, loaded }
        }
        catch (err) {
            settings = defaults
            save()
        }

        if (forceNewSettings) {
            settings = defaults
        }

        if (newIdOnLoad) {
            settings.nodeId = uuid.v4()
        }

        if (IamAScoreboard) {
            settings.IamAScoreboard = true;
        }
        save()
    }
    return settings
}

export function save() {
    window.localStorage.setItem('settings', JSON.stringify(settings))
}
