
import React from 'react'
import { connect } from 'react-redux'
import { actionCreators as system } from '../store/System'
import { actionCreators as jamTimer } from '../store/jamTimerSignalR'
import ShortClockDisplay from './shared/ShortClockDisplay';
import ShortScoreDisplay from './shared/ShortScoreDisplay';

const JamTimer = props => {
    let bs = props.boutState.current
    let left = bs.leftTeamState
    let right = bs.rightTeamState

    return (<div>
        <h1>JamTimer</h1>
        <ShortClockDisplay boutState={bs} warn={true} alert={true} />
        <ShortScoreDisplay boutState={bs} />
        {bs.phase === 0 || bs.phase === 4 //pregame or halftime
            ? <div><button onClick={() => { props.exitPregame(bs.boutId) }}>Start Lineup</button></div>
            : null
        }

        {bs.phase === 1 //lineup
            ? <div>
                <div><button onClick={() => { props.startJam(bs.boutId) }}>Start Jam</button></div>
                <div><button onClick={() => { props.startTimeout(bs.boutId) }}>Start Timeout</button></div>
            </div>
            : null
        }

        {bs.phase === 2 //jam
            ? <div><button onClick={() => { props.stopJam(bs.boutId) }}>Stop Jam</button></div>
            : null
        }

        {bs.phase === 3 //timeout
            ? <div>
                <h2>Timeout Type</h2>
                <div>
                    <button className={bs.timeoutType === 0 ? 'selected' : ''}
                        onClick={() => { props.setTimeoutType(bs.boutId, 0) }}>Official Timeout</button>
                </div>
                <div>
                    <button className={bs.timeoutType === 1 ? 'selected' : ''}
                        onClick={() => { props.setTimeoutType(bs.boutId, 1) }}>Team Timeout Left</button>
                    <button className={bs.timeoutType === 2 ? 'selected' : ''}
                        onClick={() => { props.setTimeoutType(bs.boutId, 2) }}>Team Timeout Right</button>
                </div>
                <div>
                    <button className={bs.timeoutType === 3 ? 'selected' : ''}
                        disabled={left.officialReviews < 1}
                        onClick={() => { props.setTimeoutType(bs.boutId, 3) }}>Official Review Left</button>
                    <button className={bs.timeoutType === 4 ? 'selected' : ''}
                        disabled={right.officialReviews < 1}
                        onClick={() => { props.setTimeoutType(bs.boutId, 4) }}>Official Review Right</button>
                </div>
                {
                    bs.timeoutType === 3 || bs.timeoutType === 4 ? <div>
                        <h2>Review Kept?</h2>
                        <button onClick={() => { props.setLoseReview(bs.boutId, false) }}>Keep Review</button>
                        <button onClick={() => { props.setLoseReview(bs.boutId, true) }}>Lose Review</button>
                    </div> : null
                }
                <hr />
                <div><button onClick={() => { props.stopTimeout(bs.boutId) }}>Stop Timeout</button></div>
            </div>
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
        exitPregame: (boutId) => dispatch(jamTimer.exitPregame(boutId)),
        startJam: (boutId) => dispatch(jamTimer.startJam(boutId)),
        stopJam: (boutId) => dispatch(jamTimer.stopJam(boutId)),
        startTimeout: (boutId) => dispatch(jamTimer.startTimeout(boutId)),
        stopTimeout: (boutId) => dispatch(jamTimer.stopTimeout(boutId)),
        setTimeoutType: (boutId, ttype) => dispatch(jamTimer.setTimeoutType(boutId, ttype)),
        setLoseReview: (boutId, lose) => dispatch(jamTimer.setLoseReview(boutId, lose))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(JamTimer);
