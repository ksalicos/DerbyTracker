const venueListLoaded = 'VENUE_LIST_LOADED'
const editVenue = 'EDIT_VENUE'
const venueUpdated = 'VENUE_UPDATED'

const initialState = {
    list: null,
    current: null
};

export const actionCreators = {
    listLoaded: (data) => ({ type: venueListLoaded, data: data }),
    editVenue: (data) => ({ type: editVenue, data: data }),
    venueUpdated: (data) => ({ type: venueUpdated, data: data })
};

export const reducer = (state, action) => {
    state = state || initialState
    switch (action.type) {
        case venueListLoaded:
            return { ...state, list: action.data }
        case editVenue:
            return { ...state, current: action.data }
        case venueUpdated:
            return {
                ...state, current: null,
                list: state.list.map((e) => {
                    return e.id === action.data.id ?
                        { ...action.data }
                        : e
                })
            }
        default:
            return state;
    }
};