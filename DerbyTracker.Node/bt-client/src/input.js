const watch = {
    toggleJam: []
}

export const addWatch = (binding, id, func) => {
    if (watch[binding]) {
        watch[binding][id] = func
    }
}

export const removeWatch = (binding, id) => {
    if (watch[binding][id]) {
        watch[binding][id] = null
    }
}

const keyBinding = {
    ' ': watch.toggleJam
}

export function handleKeyPress(e) {
    if (keyBinding[e.key]) {
        for (var id in keyBinding[e.key]) {
            if (keyBinding[e.key][id]) { keyBinding[e.key][id]() }
        }
    }
}