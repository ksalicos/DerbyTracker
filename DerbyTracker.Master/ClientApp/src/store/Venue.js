const venueListLoaded = 'VENUE_LIST_LOADED'
const editVenue = 'EDIT_VENUE'
const selectVenue = 'SELECT_VENUE'

const initialState = {
    list: null,
    current: null
};

export const actionCreators = {
    listLoaded: (data) => ({ type: venueListLoaded, data: data }),
    editVenue: (data) => ({ type: editVenue, data: data })
};

export const reducer = (state, action) => {
    state = state || initialState
    switch (action.type) {
        case venueListLoaded:
            return { ...state, list: action.data }
        case editVenue:
            return { ...state, current: action.data }
        case selectVenue:
            return { ...state, current: null }
        default:
            return state;
    }
};