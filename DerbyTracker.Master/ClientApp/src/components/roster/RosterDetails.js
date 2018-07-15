import React from 'react'
import { connect } from 'react-redux'
import TeamDetails from './TeamDetails'
import { Row, Col } from 'react-bootstrap'
import { actionCreators as system } from '../../store/System'
import { actionCreators as roster } from '../../store/Roster'

const RosterDetails = props => {
    return (
        <div>
            <h1>Rosters</h1>
            <Row>
                <Col sm={6}>
                    <TeamDetails team={props.bout.leftTeam} />
                    <div>
                        <button onClick={() => props.editRoster(props.bout.leftTeam, 'left')}>Edit</button>
                    </div>
                </Col>
                <Col sm={6}>
                    <TeamDetails team={props.bout.rightTeam} />
                    <div>
                        <button onClick={() => props.editRoster(props.bout.rightTeam, 'right')}>Edit</button>
                    </div>
                </Col>
            </Row>
            <button onClick={props.returnToBout}>Return</button>
        </div>
    )
};

const mapStateToProps = state => {
    return {
        roster: state.roster,
        bout: state.bout.current
    }
}

const mapDispatchToProps = dispatch => {
    return {
        returnToBout: () => dispatch(system.changeScreen('bout')),
        editRoster: (r, side) => dispatch(roster.rosterSelected(r, side))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(RosterDetails);
