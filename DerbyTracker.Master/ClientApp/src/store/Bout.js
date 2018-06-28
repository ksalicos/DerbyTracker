const boutListLoaded = 'BOUT_LIST_LOADED';
const initialState = {
    list: null,
    current: null
};

export const actionCreators = {
    listLoaded: (data) => ({ type: boutListLoaded, data: data })
};

export const reducer = (state, action) => {
    state = state || initialState;

    switch (action.type) {
        case boutListLoaded:
            return { ...state, list: action.data }
        default:
            break;
    }

    return state;
};
