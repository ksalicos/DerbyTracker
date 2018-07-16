
import React from 'react'
import { connect } from 'react-redux'
import { actionCreators as system } from '../store/System'
import { actionCreators as signalr } from '../SignalRMiddleware'
import ShortClockDisplay from './shared/ShortClockDisplay';

const JamTimer = props => {
    let bs = props.boutState.current
    console.log(props)
    return (<div>
        <h1>JamTimer</h1>
        <h2><ShortClockDisplay boutState={bs} /></h2>

        {bs.phase === 0 //pregame
            ? <div><button onClick={() => { props.exitPregame(bs.boutId) }}>Exit Pregame</button></div>
            : null
        }

        {bs.phase === 1 //lineup
            ? <div><button onClick={() => { props.startJam(bs.boutId) }}>Start Jam</button></div>
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
        startJam: (boutId) => dispatch(signalr.startJam(boutId))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(JamTimer);
