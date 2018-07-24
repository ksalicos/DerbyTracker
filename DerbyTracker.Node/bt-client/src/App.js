import React from 'react'
import LoadingScreen from './components/LoadingScreen'
import { connect } from 'react-redux'
import JamTimer from './components/JamTimer'
import LineupsTracker from './components/Lineups'
import Layout from './components/Layout'
import Scoreboard from './components/Scoreboard'

const App = props => {
  if (props.system.screen === 'loading') { return <LoadingScreen /> }
  if (props.system.screen === 'scoreboard') { return <Scoreboard /> }

  return <Layout>
    {({
      'JamTimer': (<JamTimer />),
      'LineupsTracker': (<LineupsTracker />)

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