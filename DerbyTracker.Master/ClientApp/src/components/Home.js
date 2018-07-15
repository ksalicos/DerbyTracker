import React from 'react'
import { connect } from 'react-redux'
import Layout from './Layout'
import Bout from './Bout'
import Venue from './Venue'
import Roster from './Roster'
import Nodes from './Nodes'
const Home = props => (
    <Layout>
        {({
            'bout': (<Bout />),
            'venue': (<Venue />),
            'rosters': (<Roster />),
            'nodes': (<Nodes />)
        })[props.system.screen]}
    </Layout>
);

const mapStateToProps = state => {
    return {
        system: state.system
    }
}

export default connect(mapStateToProps)(Home)