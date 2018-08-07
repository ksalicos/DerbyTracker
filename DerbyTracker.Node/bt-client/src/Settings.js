import uuid from 'uuid'
var settings = null

//debug settings
var newIdOnLoad = false
var IamAScoreboard = false

export function get() {
    if (!settings) {
        settings = JSON.parse(window.localStorage.getItem('settings'))

        if (newIdOnLoad) {
            settings.nodeId = uuid.v4()
        }
        if (IamAScoreboard) {
            settings.IamAScoreboard = true;
        }
    }
    if (!settings) {
        settings = {
            nodeId: uuid.v4()
        }
        save()
    }
    return settings
}

export function save() {
    window.localStorage.setItem('settings', JSON.stringify(settings))
}