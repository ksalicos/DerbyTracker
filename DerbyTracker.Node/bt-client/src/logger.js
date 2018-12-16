import * as settings from './Settings'
let s = settings.get()

export function log(m) {
    var xhr = new XMLHttpRequest()
    xhr.open("POST", `http://${s.remoteIp}/api/debug/trace`, true)
    var formData = new FormData();

    formData.append("message", m);
    xhr.send(formData)
    console.log(m)
}
