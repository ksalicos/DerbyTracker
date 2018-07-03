import React from 'react'
import { connect } from 'react-redux'
import { actionCreators as system } from '../../store/System'
import { actionCreators as bout } from '../../store/Bout'

const BoutList = props => (
    <div>
        <h1>Bout List</h1>
        <table>
            <tbody>
                <tr><th>Name</th><th>Date</th></tr>
                {props.bout.list.map(b => <tr>
                    <td></td>
                    <td></td>
                </tr>)}
            </tbody>
        </table>
        <button onClick={props.create}>Create</button>
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
        create: () =>
            dispatch(bout.create())
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(BoutList);