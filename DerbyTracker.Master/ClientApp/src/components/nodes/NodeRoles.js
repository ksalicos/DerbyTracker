import React from 'react'
import { connect } from 'react-redux'
import { Row, Col, Glyphicon } from 'react-bootstrap'
import { actionCreators as signalr } from '../../SignalRMiddleware'

const roles = [
    { role: 'JamTimer', label: 'Jam Timer', glyph: 'time' },
    { role: 'LineupsTracker', label: 'Lineups Tracker', glyph: 'user' },
    { role: 'ScoreKeeper', label: 'Score Keeper', glyph: 'blackboard' },
    { role: 'PenaltyTracker', label: 'Penalty Tracker', glyph: 'eye-open' },
    { role: 'Penalty Box', label: 'Penalty Box', glyph: 'inbox' },
    { role: 'HeadNSO', label: 'Head NSO', glyph: 'sunglasses' },
]

const RosterDetails = props => {
    return (
        <div>
            <div>
                Node: {props.node.connectionNumber}
            </div>
            <Row>
                {
                    roles.map((e, i) =>
                        <Col sm={2} key={i} onClick={
                            props.node.roles.includes(e.role) ? () => props.removeRole(props.node.nodeId, e.role)
                                : () => props.assignRole(props.node.nodeId, e.role)
                        }>
                            <div className='role-glyph'>
                                <Glyphicon glyph={e.glyph} className={props.node.roles.includes(e.role) ? 'in-role' : 'not-in-role'} />
                            </div>
                            <div className='role-label'>
                                {e.label}
                            </div>
                        </Col>
                    )
                }
            </Row>
        </div>
    )
};

const mapStateToProps = state => {
    return {
    }
}

const mapDispatchToProps = dispatch => {
    return {
        assignRole: (nodeId, role) => dispatch(signalr.assignRole(nodeId, role)),
        removeRole: (nodeId, role) => dispatch(signalr.removeRole(nodeId, role))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(RosterDetails);
