import React from 'react'

const second = 1000
const minute = second * 60

const foo = props => {
    let ms = props.ms
    if (ms === undefined || ms === null || ms < 0) return <span>Err...</span>
    let minutes = Math.floor(ms / minute)
    if (minutes >= 100) { return <span>99:99</span> }
    ms -= minutes * minute
    let seconds = Math.floor(ms / second)
    ms -= seconds * second

    if (minutes < 0 && seconds < 0) {
        return <span>{z(seconds)}.{z(ms)}</span>
    }
    return <span>{props.padMins ? z(minutes) : minutes}:{z(seconds)}</span>
}

function z(n) {
    return n < 10 ? '0' + n : n
}

export default foo