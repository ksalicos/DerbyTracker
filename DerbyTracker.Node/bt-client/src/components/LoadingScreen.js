import React from 'react'
import { connect } from 'react-redux'
import { actionCreators as system } from '../store/System'

const Loading = props => (
    <div id={'loadingBox'}>
        <h1>Bettie</h1>
        <h2>Client App</h2>
        <p>Loading.  Please Wait.</p>
        {props.system.initialization.signalr ? <p>SignalR GO</p> : null}
        {props.system.connectionNumber ? <p>Node Number: {props.system.connectionNumber}</p> : null}
        {
            props.system.boutId ? (props.system.boutId === '00000000-0000-0000-0000-000000000000' ? 'No Bout Assigned' : 'Bout GO!') : null
        }
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
