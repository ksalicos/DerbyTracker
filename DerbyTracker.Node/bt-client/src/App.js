import React from 'react'
import LoadingScreen from './components/LoadingScreen'
import { connect } from 'react-redux'

const App = props => {
  return (
    <div>
      {({
        'loading': (<LoadingScreen />)
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