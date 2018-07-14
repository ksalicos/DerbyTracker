import React from 'react'
import Bout from './components/Bout'
import LoadingScreen from './components/LoadingScreen'
import Venue from './components/Venue'
import { connect } from 'react-redux'

const App = props => {
    return (
        <div>
            {({
                'loading': (<LoadingScreen />),
                'bout': (<Bout />),
                'venue': (<Venue />)
            })[props.system.screen]}
        </div>
    )
}

const mapStateToProps = state => {
    return {
        system: state.system
    }
}

const mapDispatchToProps = dispatch => {
    return {}
}

export default connect(mapStateToProps, mapDispatchToProps)(App)