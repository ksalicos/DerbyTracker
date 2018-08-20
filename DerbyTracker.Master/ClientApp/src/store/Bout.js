import agent from 'superagent'
import uuid from 'uuid'

const boutListLoaded = 'BOUT_LIST_LOADED'
const runningListLoaded = 'RUNNING_BOUT_LIST_LOADED'
const boutLoaded = 'BOUT_LOADED'
const boutUpdated = 'BOUT_UPDATED'
const createBout = 'CREATE_BOUT'
const exit = 'EXIT_BOUT'
const toggleEdit = 'TOGGLE_EDIT'
const venueSelected = 'VENUE_SELECTED'
const rosterUpdated = 'ROSTER_UPDATED'
const boutRunning = 'BOUT_RUNNING'

const initialState = {
    list: null,
    current: null,
    edit: false,
    running: []
};

export const actionCreators = {
    listLoaded: (data) => ({ type: boutListLoaded, data: data }),
    runningListLoaded: (data) => ({ type: runningListLoaded, data: data }),
    boutLoaded: (data) => ({ type: boutLoaded, data: data }),
    boutUpdated: (data) => ({ type: boutUpdated, data: data }),
    create: () => ({ type: createBout }),
    exit: () => ({ type: exit }),
    toggleEdit: () => ({ type: toggleEdit }),
    venueSelected: (data) => ({ type: venueSelected, data: data }),
    rosterUpdated: (data, side) => ({ type: rosterUpdated, data: data, side: side }),
    saveBout: () => (dispatch, getState) => {
        let bout = getState().bout.current
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
        case runningListLoaded:
            return { ...state, running: action.data }
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
        case venueSelected:
            return { ...state, current: { ...state.current, venue: action.data } }
        case rosterUpdated:
            if (action.side === 'left')
                return { ...state, current: { ...state.current, left: { ...action.data } } }
            if (action.side === 'right')
                return { ...state, current: { ...state.current, right: { ...action.data } } }
            console.log('Invalid Side Set, Doing Nothing')
            break;
        case boutRunning:
            return { ...state, running: [...state.running, { boutId: action.boutId, name: action.title }] }
        default:
            break;
    }

    return state;
};

const defaultBout = {
    name: 'New Bout',
    advertisedStart: '08/26/2020 7:00 PM',
    venue: {
        name: 'The Hangar',
        city: 'Portland',
        state: 'OR'
    },
    advertisedTime: new Date().toLocaleDateString(),
    left: {
        name: 'Left Team',
        color: '&FFFFFF',
        roster: []
    },
    right: {
        name: 'Right Team',
        color: '&FFFFFF',
        roster: []
    }
}