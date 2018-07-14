import React from 'react'
import { connect } from 'react-redux'
import { actionCreators as system } from '../store/System'
import '../index.css'

const Loading = props => (
    <div id={'loadingBox'}>
        <h1>Bettie</h1>
        <h2>Bout Tracking</h2>
        <p>Loading.  Please Wait.</p>
        {props.system.initialization.signalr ? <p>SignalR GO</p> : null}
        {props.system.initialization.boutListLoaded ? <p>Bout List GO</p> : null}
        {props.system.initialization.venueListLoaded ? <p>Venue List GO</p> : null}
        {props.system.initialization.complete
            ? <p>
                <button onClick={props.go}>All Systems GO</button>
            </p>
            : null}

    </div>
);

const mapStateToProps = state => {
    return {
        system: state.system
    }
}

const mapDispatchToProps = dispatch => {
    return {
        go: () =>
            dispatch(system.changeScreen('bout'))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Loading);
