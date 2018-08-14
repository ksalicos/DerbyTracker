import React from 'react'
import { connect } from 'react-redux'
import { Row, Col, Panel } from 'react-bootstrap'
import './Shared.css'
import { phaseList } from '../../store/BoutState'
import * as clock from '../../clocks'
import TimeDisplay from './TimeDisplay'
import Circle from './Circle'
import computedbs from '../../Computed'

class GameSummary extends React.Component {
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
        clock.addWatch('gs', (t) => {
            this.setState({
                jamClock: t.jam,
                gameClock: t.game,
                lineupClock: t.lineup
            })
        })
    }
    componentWillUnmount() {
        clock.removeWatch('gs')
    }

    render() {
        let bs = this.props.boutState.current
        let left = bs.leftTeamState
        let right = bs.rightTeamState
        let data = this.props.boutState.data

        return (<Panel>
            <Panel.Body>
                <Row>
                    <Col sm={4} className='summary-timers'>
                        <div>P{bs.period}: <TimeDisplay ms={this.state.gameClock} /></div>
                        <div>
                            {
                                (() => {
                                    switch (bs.phase) {
                                        case 1:
                                            return <span>Lineup: <TimeDisplay ms={this.state.lineupClock} /></span>
                                        case 2:
                                            return <span>J{bs.jamNumber}: <TimeDisplay ms={this.state.jamClock} /></span>
                                        default: return phaseList[bs.phase]
                                    }
                                })()
                            }
                        </div>
                    </Col>
                    <Col sm={1} className='summary-score'>
                        {computedbs(bs).leftScore}
                    </Col>
                    <Col sm={2}>
                        <div>
                            {new Array(3).fill().map((e, i) => <div key={i} className='summary-timeout' >
                                <Circle color={i <= left.timeOutsRemaining - 1 ? data['left'].color : '#555'} />
                            </div>)}
                            {
                                left.officialReviews > 0
                                    ? <div className='summary-timeout i-mean-review'>
                                        <Circle color={data['left'].color} />
                                    </div>
                                    : <div className='summary-timeout i-mean-review'>
                                        <Circle color='#555' />
                                    </div>
                            }
                        </div>
                        <div>
                            {new Array(right.timeOutsRemaining).fill().map((e, i) => <div key={i} className='summary-timeout' >
                                <Circle color={i <= right.timeOutsRemaining - 1 ? data['right'].color : '#555'} />
                            </div>)}
                            {
                                right.officialReviews > 0
                                    ? <div className='summary-timeout i-mean-review'>
                                        <Circle color={data['right'].color} />
                                    </div>
                                    : <div className='summary-timeout i-mean-review'>
                                        <Circle color='#555' />
                                    </div>
                            }
                        </div>
                    </Col>
                    <Col sm={1} className='summary-score'>
                        {computedbs(bs).rightScore}
                    </Col>
                </Row>
            </Panel.Body>
        </Panel>)
    }
}

const mapStateToProps = state => {
    return {
        boutState: state.boutState
    }
}

export default connect(mapStateToProps)(GameSummary);
