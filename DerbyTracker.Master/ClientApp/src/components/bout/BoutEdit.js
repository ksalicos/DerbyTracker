import React from 'react'
import { connect } from 'react-redux'
import { actionCreators as system } from '../../store/System'
import { actionCreators as bout } from '../../store/Bout'

const BoutEdit = props => (
    <div>
        <h1>Bout Edit</h1>

        <button onClick={props.edit}>Edit</button>
        <button onClick={props.exit}>Exit</button>
        <button>Run</button>
    </div>
);

const mapStateToProps = state => {
    return {
        bout: state.bout
    }
}

const mapDispatchToProps = dispatch => {
    return {
        go: () =>
            dispatch(system.changeScreen('home')),
        exit: () =>
            dispatch(bout.exit()),
        edit: () =>
            dispatch(bout.edit)
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(BoutEdit);