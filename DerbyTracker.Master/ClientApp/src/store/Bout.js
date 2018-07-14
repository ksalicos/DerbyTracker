import agent from 'superagent'
import uuid from 'uuid'

const boutListLoaded = 'BOUT_LIST_LOADED'
const boutLoaded = 'BOUT_LOADED'
const boutUpdated = 'BOUT_UPDATED'
const createBout = 'CREATE_BOUT'
const exit = 'EXIT_BOUT'
const toggleEdit = 'TOGGLE_EDIT'
const selectVenue = 'SELECT_VENUE'

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
    toggleEdit: () => ({ type: toggleEdit }),
    selectVenue: (data) => ({ type: selectVenue, data: data }),
    saveBout: () => (dispatch, getState) => {
        let state = getState().bout
        let bout = {
            boutId: state.current.boutId,
            name: state.current.name,
            advertisedStart: state.current.advertisedStart,
            venue: state.current.venue
        }
        agent.post('/api/bout/save')
            .send(bout)
            .set('Accept', 'application/json')
            .then((r) => {
                dispatch(actionCreators.toggleEdit())
            })
    }
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
                newList.push({
                    id: action.data.boutId,
                    name: action.data.name,
                    timeStamp: new Date().toLocaleDateString()
                })
            }
            return { ...state, current: { ...state.current, ...action.data }, list: newList }
        case createBout:
            return { ...state, current: { ...defaultBout, boutId: uuid.v4() }, edit: true }
        case exit:
            return { ...state, current: null }
        case toggleEdit:
            return { ...state, edit: !state.edit }
        case selectVenue:
            return { ...state, current: { ...state.current, venue: action.data } }
        default:
            break;
    }

    return state;
};

const defaultBout = {
    name: 'New Bout',
    venue: null,
    advertisedTime: new Date().toLocaleDateString()
}