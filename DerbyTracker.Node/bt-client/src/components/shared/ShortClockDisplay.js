
import React from 'react'
import moment from 'moment'
import TimeDisplay from './TimeDisplay'
import { phase } from '../../store/BoutState'

class ShortClockDisplay extends React.Component {
    constructor(props) {
        super(props);

        //set these to timespans in milliseconds
        this.state = {
            gameClock: null,
            jamClock: null,
            lineupClock: null
        }

        this.handleChange = this.handleChange.bind(this)
        this.JamClock = this.JamClock.bind(this)
        this.setClocks = this.setClocks.bind(this)
    }

    componentDidMount() {
        this.setClocks()
    }




    render() {
        let bs = this.props.boutState
        switch (bs.phase) {
            case 0: //Pregame
                return <span>{phase[bs.phase]}</span>
            case 1: //Lineup,
                if (bs.clockRunning) {
                    return <span>
                        Jam: {this.props.boutState.jam} <TimeDisplay ms={this.state.jamClock} />
                    </span>
                }
                return <span>
                    Jam: {this.props.boutState.jam} <TimeDisplay ms={120000} />
                </span>
            case 2: //Jam
                return <span>
                    Jam: {this.props.boutState.jam} <TimeDisplay ms={this.state.jamClock} />
                </span>
            case 3: //Timeout
                return <span>
                    Jam: {this.props.boutState.jam} {phase[bs.phase]}
                </span>
            case 4: //Halftime
                break
            case 5: //UnofficialScore
                return <span>{phase[bs.phase]}</span>
            case 6: //OfficialScore
                return <span>{phase[bs.phase]}</span>
            default:
                return <span>Err...</span>
        }
    }

    setClocks() {
        let now = moment()
        let jamClock = this.JamClock(now)

        this.setState({ jamClock: jamClock })
        setTimeout(this.setClocks, 500)
    }

    //GameClock() => ClockRunning ? GameTimeElapsed + (DateTime.Now - LastClockStart) : GameTimeElapsed;
    JamClock(now) {
        let bs = this.props.boutState
        //TODO: pull jam duration from ruleset
        let dif = Math.max(120000 - now.diff(bs.jamStart), 0)
        return dif
    }
    //=> DateTime.Now - JamStart;
    //public TimeSpan LineupClock() => DateTime.Now - LineupStart;

    handleChange(event) {
        const target = event.target;
        this.setState({ [target.name]: target.value, saved: false });
    }
}

export default ShortClockDisplay