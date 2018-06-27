import React from 'react';
import { connect } from 'react-redux';

const Home = props => (
    <div>
        I am home.
    </div>
);

export default connect()(Home);
