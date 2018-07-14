import React from 'react'
import { connect } from 'react-redux'
import Layout from './Layout'
import Bout from './Bout'
import Venue from './Venue'

const Home = props => (
    <Layout>
        {({
            'bout': (<Bout />),
            'venue': (<Venue />)
        })[props.system.screen]}
    </Layout>
);

const mapStateToProps = state => {
    return {
        system: state.system
    }
}

export default connect(mapStateToProps)(Home)