import React from 'react'
import LoadingScreen from './components/LoadingScreen'
import { connect } from 'react-redux'
import JamTimer from './components/JamTimer'
import LineupsTracker from './components/Lineups'
import Layout from './components/Layout'
import Scoreboard from './components/Scoreboard'
import PenaltyTracker from './components/PenaltyTracker'
import ScoreKeeper from './components/ScoreKeeper'
import PenaltyBoxTimer from './components/PenaltyBoxTimer'
import * as input from './input'

class App extends React.Component {
  componentDidMount() {
    document.addEventListener('keydown', input.handleKeyPress);
  }

  componentWillUnmount() {
    document.removeEventListener('keydown', input.handleKeyPress);
  }

  render() {
    if (this.props.system.screen === 'loading') { return <LoadingScreen /> }
    if (this.props.system.screen === 'scoreboard') { return <Scoreboard /> }

    return <Layout>
      {({
        'JamTimer': (<JamTimer />),
        'LineupsTracker': (<LineupsTracker />),
        'PenaltyTracker': (<PenaltyTracker />),
        'ScoreKeeper': (<ScoreKeeper />),
        'PenaltyBoxTimer': (<PenaltyBoxTimer />)
      })[this.props.system.screen]}
    </Layout>
  }
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