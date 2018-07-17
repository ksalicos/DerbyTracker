
import React from 'react'
import { connect } from 'react-redux'
import { actionCreators as system } from '../store/System'
import { actionCreators as signalr } from '../SignalRMiddleware'
import ShortClockDisplay from './shared/ShortClockDisplay';

const JamTimer = props => {
    let bs = props.boutState.current
    return (<div>
        <h1>JamTimer</h1>
        <ShortClockDisplay boutState={bs} />

        {bs.phase === 0 || bs.phase === 4 //pregame or halftime
            ? <div><button onClick={() => { props.exitPregame(bs.boutId) }}>Start Lineup</button></div>
            : null
        }

        {bs.phase === 1 //lineup
            ? <div><button onClick={() => { props.startJam(bs.boutId) }}>Start Jam</button></div>
            : null
        }

        {bs.phase === 2 //jam
            ? <div><button onClick={() => { props.stopJam(bs.boutId) }}>Stop Jam</button></div>
            : null
        }
    </div>)
}

const mapStateToProps = state => {
    return {
        system: state.system,
        boutState: state.boutState
    }
}

const mapDispatchToProps = dispatch => {
    return {
        go: () => dispatch(system.changeScreen('bout')),
        exitPregame: (boutId) => dispatch(signalr.exitPregame(boutId)),
        startJam: (boutId) => dispatch(signalr.startJam(boutId)),
        stopJam: (boutId) => dispatch(signalr.stopJam(boutId))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(JamTimer);
