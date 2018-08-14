import React from 'react'
import { Row, Col } from 'react-bootstrap'

//props
// penalties - should already be filtered

const Queue = props => {
    return (<Row>
        {
            props.penalties.map((e, i) => <Col sm={2} key={i} className='queue-item'>
                <div className='queue-number'>#{e.number} {e.seconds}s</div>
            </Col>)
        }
    </Row>)
}

export default Queue