import React from 'react'
import { Row, Col } from 'react-bootstrap'

const TeamDetails = props => {
    if (!props.team) {
        return <div>Team Not Defined</div>
    }
    return (
        <div>
            <div className='poppy'>{props.team.name}</div>
            {
                props.team.roster.map((e, i) => {
                    return <Row key={i}>
                        <Col sm={2}>{e.number}</Col>
                        <Col sm={4}>{e.name}</Col>
                    </Row>
                })
            }
        </div>
    )
};

export default TeamDetails
