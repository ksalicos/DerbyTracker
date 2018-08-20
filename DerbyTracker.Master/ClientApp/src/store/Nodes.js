const nodeConnected = 'NODE_CONNECTED'
const listLoaded = 'NODE_LIST_LOADED'
const rolesUpdated = 'NODE_ROLES_UPDATED'
const nodeJoinedBout = 'NODE_JOINED_BOUT'

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
            return state.list.some((e) => { return e.nodeId === action.data.nodeId })
                ? { ...state }
                : { ...state, list: [...state.list, action.data] }
        case rolesUpdated:
            let newList = state.list.map((e) => {
                return e.nodeId === action.nodeId ? { ...e, roles: action.newRoles } : e
            })
            return { ...state, list: newList }
        case nodeJoinedBout:
            return {
                list: state.list.map(e =>
                    e.nodeId === action.nodeId ? { ...e, boutId: action.boutId } : e
                )
            }
        default:
            return state
    }
};