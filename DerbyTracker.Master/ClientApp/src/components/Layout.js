import React from 'react';
import { Col, Grid, Row } from 'react-bootstrap';

export default props => (
    <Grid fluid>
        <Row>
            <Col sm={3}>
                <p>STUFF</p>
            </Col>
            <Col sm={9}>
                {props.children}
            </Col>
        </Row>
    </Grid>
);
