import React from 'react'
import LoadingScreen from './components/LoadingScreen'
import Home from './components/Home'
import { connect } from 'react-redux'

const App = props => {
    return props.system.screen === 'loading' ? <LoadingScreen /> : <Home />
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