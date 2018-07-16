import uuid from 'uuid'
var settings = null
var newIdOnLoad = false;

export function get() {
    if (!settings) {
        settings = JSON.parse(window.localStorage.getItem('settings'))
        if (newIdOnLoad) {
            settings.nodeId = uuid.v4()
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