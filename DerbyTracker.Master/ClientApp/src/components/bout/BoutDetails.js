import React from 'react'
import { connect } from 'react-redux'
import { actionCreators as bout } from '../../store/Bout'
import { actionCreators as signalr } from '../../SignalRMiddleware'

import Moment from 'react-moment';
import MatchupText from './MatchupText'

const BoutDetails = props => {
    let bout = props.bout.current
    if (!bout) {
        console.log('attempted to display BoutDetails with no current bout loaded')
        return null
    }

    return (
        <div>
            <div>
                <h1>Bout Details</h1>
                <h2>{bout.name}</h2>
                <MatchupText bout={bout} />
                {bout.venue ? <p>{bout.venue.name} {bout.venue.city}, {bout.venue.state}</p>
                    : <p>No Venue Set</p>}
                <p><Moment format="MMM DD YYYY, h:mmA">{bout.advertisedStart}</Moment></p>
                <button onClick={props.edit}>Edit</button>
                <button onClick={props.exit}>Exit</button>
            </div>
            <div>
                <button onClick={() => { props.runBout(bout.boutId) }}>Run</button>
            </div>
        </div>
    )
};

const mapStateToProps = state => {
    return {
        bout: state.bout
    }
}

const mapDispatchToProps = dispatch => {
    return {
        exit: () => dispatch(bout.exit()),
        edit: () => dispatch(bout.toggleEdit()),
        runBout: (b) => dispatch(signalr.runBout(b))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(BoutDetails);