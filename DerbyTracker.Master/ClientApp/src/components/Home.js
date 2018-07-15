import React from 'react'
import { connect } from 'react-redux'
import Layout from './Layout'
import Bout from './Bout'
import Venue from './Venue'
import Roster from './Roster'

const Home = props => (
    <Layout>
        {({
            'bout': (<Bout />),
            'venue': (<Venue />),
            'rosters': (<Roster />)
        })[props.system.screen]}
    </Layout>
);

const mapStateToProps = state => {
    return {
        system: state.system
    }
}

export default connect(mapStateToProps)(Home)