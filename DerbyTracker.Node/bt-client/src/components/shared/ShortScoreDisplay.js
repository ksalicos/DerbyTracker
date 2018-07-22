import React from 'react'
import { Row, Col } from 'react-bootstrap'

const ShortScoreDisplay = props => {
    let left = props.boutState.leftTeamState
    let right = props.boutState.rightTeamState

    let leftTO = `${left.timeOutsRemaining}${left.officialReviews > 0 ? `+${left.officialReviews}` : ''}`
    let rightTO = `${right.timeOutsRemaining}${right.officialReviews > 0 ? `+${right.officialReviews}` : ''}`

    return (<Row>
        <Col sm={2}>Left Team Name</Col>
        <Col sm={2}>Timeouts: {leftTO}</Col>
        <Col sm={2}>Score: {left.score}</Col>
        <Col sm={2}>Right Team Name</Col>
        <Col sm={2}>Timeouts: {rightTO}</Col>
        <Col sm={2}>Score: {right.score}</Col>
    </Row>)
}

export default ShortScoreDisplay