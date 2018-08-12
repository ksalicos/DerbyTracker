import { initializeBoutState, updateBoutState } from './BoutState'
const setReady = 'SET_READY'
const setJam = 'SET_JAM'

export const actionCreators = {
    setReady: (r) => ({ type: setReady, ready: r }),
    setJam: (j) => ({ type: setJam, jamNumber: j })
}

const initialState = {
    ready: false,
    index: 0,
    lastIdx: 0
}

export const reducer = (state, action) => {
    state = state || initialState

    switch (action.type) {
        case setReady:
            console.log(action)
            return { ...state, ready: action.ready }
        case initializeBoutState:
        case updateBoutState:
            let newIdx = action.boutState.jams.length - 1
            if (newIdx === state.lastIdx) return state
            if (!state.ready) return { ...state, lastIdx: newIdx }
            return { ...state, ready: false, index: newIdx, lastIdx: newIdx }
        case setJam:
            return { ...state, index: action.jamNumber }
        default:
            return state
    }
}