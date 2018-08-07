import React from 'react'
import { Row, Col, Button, Modal } from 'react-bootstrap'

// Props
// show (bool)
// close (function)
// selectSkater (function(number))
// roster (array<skater>)
// lineup (array<skater>)

const SkaterSelect = props => <Modal show={props.show} animation={false}>
    <Modal.Header closeButton onHide={props.close}>
        <Modal.Title>Select Skater</Modal.Title>
    </Modal.Header>
    <Modal.Body>
        {
            props.lineup && props.lineup.length > 0 ? <div>
                <h3>Current Lineup</h3>
                <Row>
                    {props.lineup.map(e => {
                        return (<Col sm={2} key={e.number}>
                            <Button block onClick={() => props.selectSkater(e.number)}>{e.number}</Button>
                        </Col>)
                    })}
                </Row>
            </div>
                : null
        }

        <h3>Full Roster</h3>
        <Row>
            {props.roster.map(e => {
                return (<Col sm={2} key={e.number}>
                    <Button block onClick={() => props.selectSkater(e.number)}>{e.number}</Button>
                </Col>)
            })}
        </Row>
    </Modal.Body>
</Modal>

export default SkaterSelect