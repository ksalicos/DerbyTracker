
import React from 'react'
import moment from 'moment'
import TimeDisplay from './TimeDisplay'
import { phase } from '../../store/BoutState'
import { Row, Col } from 'react-bootstrap'

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
        if (bs.phase === 0 || bs.phase >= 3)
            return (
                < Row >
                    <Col sm={6}>
                        <span>{phase[bs.phase]}</span>
                    </Col>
                    <Col sm={2}>
                        Period:
                    </Col>
                    <Col sm={1}>
                        {this.props.boutState.period}
                    </Col>
                    <Col sm={3}>
                        <TimeDisplay ms={this.state.gameClock} />
                    </Col>
                </Row >)

        let jamTimeColor = 'default'
        if (this.props.alert && this.state.jamClock === 0) {
            jamTimeColor = 'timeAlert'
        } else if (this.props.warn && this.state.jamClock < 10000) {
            jamTimeColor = 'timeWarning'
        }

        return (
            < Row >
                <Col sm={2}>
                    <span>{phase[bs.phase]}</span>
                </Col>
                <Col sm={2}>
                    <span>{bs.jam}</span>
                </Col>
                <Col sm={2}>
                    <TimeDisplay ms={bs.phase === 1
                        ? this.state.lineupClock
                        : this.state.jamClock}
                        color={jamTimeColor}
                    />
                </Col>
                <Col sm={2}>
                    Period:
                </Col>
                <Col sm={1}>
                    {this.props.boutState.period}
                </Col>
                <Col sm={3}>
                    <TimeDisplay ms={this.state.gameClock} />
                </Col>
            </Row >)
    }

    setClocks() {
        let now = moment()
        let bs = this.props.boutState
        let jamClock = Math.max(120000 - now.diff(bs.jamStart), 0)
        let lineupClock = Math.max(30000 - now.diff(bs.lineupStart), 0)
        let gameClock = bs.clockRunning
            ? Math.max(1800000 - (bs.gameClockElapsed + now.diff(bs.lastClockStart)), 0)
            : 1800000 - bs.gameClockElapsed
        this.setState({ jamClock: jamClock, lineupClock: lineupClock, gameClock: gameClock })
        setTimeout(this.setClocks, 500)
    }

    //GameClock() => ClockRunning ? GameTimeElapsed + (DateTime.Now - LastClockStart) : GameTimeElapsed;
    JamClock(now) {
        let bs = this.props.boutState
        //TODO: pull jam duration from ruleset
        let dif = Math.max(120000 - now.diff(bs.jamStart), 0)
        return dif
    }

    handleChange(event) {
        const target = event.target;
        this.setState({ [target.name]: target.value, saved: false });
    }
}

export default ShortClockDisplay