﻿import React from 'react'
import Bout from './components/Bout'
import LoadingScreen from './components/LoadingScreen'
import { connect } from 'react-redux'

const App = props => {
    return (
        <div>
            {({
                'loading': (<LoadingScreen />),
                'bout': (<Bout />)
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