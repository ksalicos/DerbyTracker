import React from 'react';
import { Col, Grid, Row } from 'react-bootstrap';
import NavMenu from './NavMenu';

export default props => (
    <Grid className='fill-height'>
        <Row className='fill-height'>
            <Col sm={2} className='left-box'>
                <NavMenu />
            </Col>
            <Col sm={3} className='right-box'>
                {props.children}
            </Col>
        </Row>
    </Grid>
);
