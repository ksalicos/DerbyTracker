import React from 'react';
import { Grid } from 'react-bootstrap';
import NavMenu from './NavMenu';

export default props => (
    <div>
        <NavMenu />
        <Grid className='fill-height'>
            {props.children}
        </Grid>
    </div>
);
