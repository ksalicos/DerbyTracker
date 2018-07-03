import React from 'react'
import { connect } from 'react-redux'
import { actionCreators as system } from '../store/System'
import BoutList from './bout/BoutList'
import BoutDetails from './bout/BoutDetails'
import BoutEdit from './bout/BoutEdit'

const Bout = props => {
    console.log(props)

    return (
        <div>
            <h1>BOUT</h1>
            {
                props.bout.current
                    ? (props.bout.edit ? <BoutEdit /> : <BoutDetails />)
                    : <BoutList />
            }
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
        go: () =>
            dispatch(system.changeScreen('home'))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Bout);
