import moment from 'moment'

var rules
export const setRules = r => {
    rules = {
        jamDuration: r.jamDurationSeconds * 1000,
        lineupDuration: r.lineupDurationSeconds * 1000,
        periodDuration: r.periodDurationSeconds * 1000
    }
}

var clocks = {
    game: 0,
    jam: 0,
    lineup: 0
}

var gameClock = {
    elapsedMs: 0,
    lastStarted: moment(),
    running: false
}

var jamClockStarted = moment();
var lineupClockStarted = moment();

export const setClocks = bs => {
    console.log(bs)
    gameClock = {
        running: bs.gameClock.running,
        elapsedMs: bs.gameClock.elapsedMs,
        lastStarted: moment(bs.gameClock.lastStarted)
    }
    lineupClockStarted = moment(bs.lineupStart)
    jamClockStarted = moment(bs.jamStart)
}

var watch = {
    game: {},
    jam: {},
    lineup: {}
}

export const addWatch = (clock, id, func) => {
    watch[clock][id] = func

}
export const removeWatch = (clock, id) => { watch[clock][id] = null }

function tick() {
    setTimeout(tick, 100)
    if (!rules) { return }

    let now = moment()
    clocks = {
        game: gameClock.running
            ? Math.max(rules.periodDuration - (gameClock.elapsedMs + now.diff(gameClock.lastStarted)), 0)
            : rules.periodDuration - gameClock.elapsedMs,
        jam: Math.max(rules.jamDuration - now.diff(jamClockStarted), 0),
        lineup: Math.max(rules.lineupDuration - now.diff(lineupClockStarted), 0)
    }
    notify(watch.game, clocks.game)
    notify(watch.jam, clocks.jam)
    notify(watch.lineup, clocks.lineup)
}
tick()

const notify = (list, time) => {
    for (var id in list) {
        list[id](time)
    }
}