import React from 'react'
import { connect } from 'react-redux'
import RosterDetails from './roster/RosterDetails'
import RosterEdit from './roster/RosterEdit'

const Roster = props => {
    return (
        <div>
            {
                props.roster.current
                    ? <RosterEdit />
                    : <RosterDetails />
            }
        </div>
    )
};

const mapStateToProps = state => {
    return {
        roster: state.roster,
        bout: state.bout
    }
}

export default connect(mapStateToProps)(Roster);
