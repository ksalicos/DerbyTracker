var _state = null
export default function get(state) {
    if (state !== _state) {
        _state = state
        compute()
    }
    return _computed
}

var _computed = {
    jamscores: [],
    leftScore: 0,
    rightScore: 0,
    queue: []
}

const compute = () => {
    _computed.jamscores = _state.jams.map(e => ({
        left: e['left'].passes.reduce((t, e) => { return t + e.score }, 0),
        right: e['right'].passes.reduce((t, e) => { return t + e.score }, 0)
    }))

    _computed.leftScore = _computed.jamscores.reduce((t, e) => {
        return t + e.left
    }, 0)
    _computed.rightScore = _computed.jamscores.reduce((t, e) => {
        return t + e.right
    }, 0)

    _computed.queue = []
    _state.penalties.forEach((p, i) => {
        let q = _computed.queue.find(e => p.number === e.number && p.team === e.team)
        if (q) {
            q.seconds += p.secondsOwed
        }
        else {
            q = { number: p.number, seconds: p.secondsOwed, team: p.team }
            _computed.queue.push(q)
        }
    })
}
