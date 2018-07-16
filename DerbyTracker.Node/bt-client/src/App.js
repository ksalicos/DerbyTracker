import React from 'react'
import LoadingScreen from './components/LoadingScreen'
import { connect } from 'react-redux'
import JamTimer from './components/JamTimer'
import Layout from './components/Layout'

const App = props => {
  if (props.system.screen === 'loading') { return <LoadingScreen /> }

  return <Layout>
    {({
      'JamTimer': (<JamTimer />)
    })[props.system.screen]}
  </Layout>
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