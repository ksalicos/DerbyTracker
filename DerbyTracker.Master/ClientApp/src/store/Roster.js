const rosterSelected = 'ROSTER_SELECTED'
const rosterListLoaded = 'ROSTER_LIST_LOADED'
const rosterUpdated = 'ROSTER_UPDATED'

const initialState = {
    list: null,
    current: null,
    selectedSide: null
};

export const actionCreators = {
    listLoaded: (data) => ({ type: rosterListLoaded, data: data }),
    rosterSelected: (data, side) => ({ type: rosterSelected, data: data, side: side })
};

export const reducer = (state, action) => {
    state = state || initialState
    switch (action.type) {
        case rosterListLoaded:
            return { ...state, list: action.data }
        case rosterSelected:
            return { ...state, current: action.data, selectedSide: action.side }
        case rosterUpdated:
            return { ...state, current: null, selectedSide: null }
        default:
            return state;
    }
};