const boutListLoaded = 'BOUT_LIST_LOADED'
const boutLoaded = 'BOUT_LOADED'
const boutUpdated = 'BOUT_UPDATED'
const createBout = 'CREATE_BOUT'
const exit = 'EXIT_BOUT'
const toggleEdit = 'TOGGLE_EDIT'

const initialState = {
    list: null,
    current: null,
    edit: false
};

export const actionCreators = {
    listLoaded: (data) => ({ type: boutListLoaded, data: data }),
    boutLoaded: (data) => ({ type: boutLoaded, data: data }),
    boutUpdated: (data) => ({ type: boutUpdated, data: data }),
    create: () => ({ type: createBout }),
    exit: () => ({ type: exit }),
    toggleEdit: () => ({ type: toggleEdit })
};

export const reducer = (state, action) => {
    state = state || initialState;

    switch (action.type) {
        case boutListLoaded:
            return { ...state, list: action.data }
        case boutLoaded:
            return { ...state, current: action.data }
        case boutUpdated:
            let newList = state.list.map((e) => e.id === state.current.boutId
                ? { ...e, id: action.data.boutId, name: action.data.name } : e)
            let element = newList.find((e) => e.id === state.current.boutId)
            if (!element) {
                newList.push({ id: action.data.boutId, name: action.data.name, timeStamp: new Date().toLocaleDateString() })
            }
            return { ...state, current: action.data, list: newList }
        case createBout:
            return { ...state, current: defaultBout, edit: true }
        case exit:
            return { ...state, current: null }
        case toggleEdit:
            return { ...state, edit: !state.edit }
        default:
            break;
    }

    return state;
};

const defaultBout = {
    boutId: '00000000-0000-0000-0000-000000000000',
    name: 'New Bout',
    venue: 'The Hangar',
    advertisedTime: new Date().toLocaleDateString()
}