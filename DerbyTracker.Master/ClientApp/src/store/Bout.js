const boutListLoaded = 'BOUT_LIST_LOADED'
const createBout = 'CREATE_BOUT'
const exit = 'EXIT_BOUT'
const edit = 'EDIT_BOUT'

const initialState = {
    list: null,
    current: null,
    edit: false
};

export const actionCreators = {
    listLoaded: (data) => ({ type: boutListLoaded, data: data }),
    create: () => ({ type: createBout }),
    exit: () => ({ type: exit }),
    edit: () => ({ type: edit })
};

export const reducer = (state, action) => {
    state = state || initialState;

    switch (action.type) {
        case boutListLoaded:
            return { ...state, list: action.data }
        case createBout:
            return { ...state, current: defaultBout }
        case exit:
            return { ...state, current: null }
        case edit:
            return { ...state, edit: true }
        default:
            break;
    }

    return state;
};

const defaultBout = {}