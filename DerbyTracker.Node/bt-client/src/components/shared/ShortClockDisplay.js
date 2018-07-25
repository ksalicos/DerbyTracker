
import React from 'react'
import TimeDisplay from './TimeDisplay'
import { phaseList } from '../../store/BoutState'
import { Row, Col } from 'react-bootstrap'
import * as clock from '../../clocks'

class ShortClockDisplay extends React.Component {
    constructor(props) {
        super(props);

        //set these to timespans in milliseconds
        this.state = {
            gameClock: null,
            jamClock: null,
            lineupClock: null
        }
    }

    componentDidMount() {
        clock.addWatch('scd', (t) => {
            this.setState({
                jamClock: t.jam,
                gameClock: t.game,
                lineupClock: t.lineup
            })
        })
    }
    componentWillUnmount() {
        clock.removeWatch('scd')
    }

    render() {
        let bs = this.props.boutState
        if (bs.phase === 0 || bs.phase >= 3)
            return (
                < Row >
                    <Col sm={6}>
                        <span>{phaseList[bs.phase]}</span>
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

        let alert = (bs.phase === 1 && this.state.lineupClock === 0)
            || (bs.phase === 2 && this.state.jamClock === 0)
        let warn = (bs.phase === 1 && this.state.lineupClock < 10000)
            || (bs.phase === 2 && this.state.jamClock < 10000)

        if (alert) {
            jamTimeColor = 'timeAlert'
        } else if (warn) {
            jamTimeColor = 'timeWarning'
        }

        return (
            < Row >
                <Col sm={2}>
                    <span>{phaseList[bs.phase]}</span>
                </Col>
                <Col sm={2}>
                    <span>{bs.jamNumber}</span>
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
}

export default ShortClockDisplay