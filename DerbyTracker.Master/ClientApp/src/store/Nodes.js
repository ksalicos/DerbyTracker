const nodeConnected = 'NODE_CONNECTED'
const listLoaded = 'NODE_LIST_LOADED'
const rolesUpdated = 'NODE_ROLES_UPDATED'

const initialState = {
    list: []
};

export const actionCreators = {
    listLoaded: (data) => ({ type: listLoaded, data: data })
};

export const reducer = (state, action) => {
    state = state || initialState
    switch (action.type) {
        case listLoaded:
            return { ...state, list: action.data }
        case nodeConnected:
            return { ...state, list: [...state.list, action.data] }
        case rolesUpdated:
            let newList = state.list.map((e) => {
                return e.nodeId == action.nodeId ? { ...e, roles: action.newRoles } : e
            })
            return { ...state, list: newList }
        default:
            return state
    }
};