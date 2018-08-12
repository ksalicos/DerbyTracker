import './JamTimer.css'
import React from 'react'
import { connect } from 'react-redux'
import { actionCreators as system } from '../store/System'
import { actionCreators as jamTimer } from '../store/jamTimerSignalR'
import { Col, Row, Button, Panel } from 'react-bootstrap';
import TimeDisplay from './shared/TimeDisplay'
import * as clock from '../clocks'
import * as input from '../input'
import { phaseList } from '../store/BoutState'
import Circle from './shared/Circle'

class JamTimer extends React.Component {
    constructor(props) {
        super(props)

        this.state = {
            gameClock: null,
            jamClock: null,
            lineupClock: null
        }
        this.startStopJam = this.startStopJam.bind(this);
    }

    componentDidMount() {
        clock.addWatch('jt', (clock) => {
            this.setState({
                jamClock: clock.jam,
                gameClock: clock.game,
                lineupClock: clock.lineup
            })
        })
        input.addWatch('toggleJam', 'jt', this.startStopJam)
    }
    componentWillUnmount() {
        clock.removeWatch('jt')
        input.removeWatch('toggleJam', 'jt')
    }

    startStopJam() {
        let bs = this.props.boutState.current
        switch (bs.phase) {
            case 1:
                this.props.startJam(bs.boutId)
                break
            case 2:
                this.props.stopJam(bs.boutId)
                break
            default:
                break
        }
    }

    render() {
        let props = this.props
        if (!props.boutState.current) return <p>Loading</p>

        let bs = props.boutState.current
        let left = bs.leftTeamState
        let right = bs.rightTeamState

        let lto = bs.leftTeamState.timeOutsRemaining
        let rto = bs.rightTeamState.timeOutsRemaining
        let lcolor = this.props.boutState.data.left.color
        let rcolor = this.props.boutState.data.right.color
        let used = '#555555'

        return (<div>
            <Row>
                <Col xs={2} sm={1}>
                    <div className='timeout'><Circle className='timeout-circle' color={lto >= 3 ? lcolor : used} /></div>
                    <div className='timeout'><Circle className='timeout-circle' color={lto >= 2 ? lcolor : used} /></div>
                    <div className='timeout'><Circle className='timeout-circle' color={lto >= 1 ? lcolor : used} /></div>
                    <div className='timeout official-review'><Circle className='timeout-circle' color={bs.leftTeamState.officialReviews > 0 ? lcolor : used} /></div>
                </Col>
                <Col xs={8} sm={10}>
                    <Panel><Panel.Body>
                        <Row className='period-display'>
                            <Col md={12} lgOffset={1} lg={5}>
                                P{bs.period}: <TimeDisplay ms={this.state.gameClock} />
                            </Col>
                            {
                                bs.phase === 1
                                    ? <Col md={12} lg={5}>
                                        Lineup: <TimeDisplay ms={this.state.lineupClock} />
                                    </Col>
                                    : null
                            }
                            {
                                bs.phase === 2
                                    ? <Col md={12} lg={4}>
                                        J{bs.jamNumber}: <TimeDisplay ms={this.state.jamClock} />
                                    </Col>
                                    : null
                            }
                            {
                                bs.phase === 0 || bs.phase >= 3
                                    ? <Col md={12} lg={4}>
                                        {phaseList[bs.phase]}
                                    </Col>
                                    : null
                            }
                        </Row>
                    </Panel.Body></Panel>
                    <Panel><Panel.Body>

                        <Row>
                            <Col sm={12} mdOffset={2} md={8} lgOffset={3} lg={6}>
                                {bs.phase === 0 || bs.phase === 4 //pregame or halftime
                                    ? <Button block bsSize='large' onClick={() => { props.exitPregame(bs.boutId) }}>Start Lineup</Button>
                                    : null
                                }

                                {bs.phase === 1 //lineup
                                    ? <div>
                                        <Button block bsSize='large' bsStyle='primary' onClick={() => { props.startJam(bs.boutId) }}>Start Jam</Button>
                                        <Button block bsSize='large' onClick={() => { props.startTimeout(bs.boutId) }}>Start Timeout</Button>
                                        {
                                            this.state.gameClock === 0
                                                ? <Button block bsSize='large' onClick={() => { props.endPeriod(bs.boutId) }}>End Period</Button>
                                                : null
                                        }
                                    </div>
                                    : null
                                }

                                {bs.phase === 2 //jam
                                    ? <Button block bsSize='large' bsStyle='primary' onClick={() => { props.stopJam(bs.boutId) }}>Stop Jam</Button>
                                    : null
                                }

                                {bs.phase === 3 //timeout
                                    ? <div>
                                        <div className='period-display'>Timeout Type</div>
                                        <Row>
                                            <Col sm={12} className='button-column'>
                                                <Button block bsSize='large' bsStyle={bs.timeoutType === 0 ? 'primary' : 'default'}
                                                    onClick={() => { props.setTimeoutType(bs.boutId, 0) }}>Official Timeout</Button>
                                            </Col>
                                        </Row>
                                        <Row>
                                            <Col xs={11} sm={6} className='button-column'>
                                                <Button block bsSize='large' bsStyle={bs.timeoutType === 1 ? 'primary' : 'default'}
                                                    disabled={left.timeOutsRemaining < 1}
                                                    onClick={() => { props.setTimeoutType(bs.boutId, 1) }}>Team Timeout Left</Button>
                                            </Col>
                                            <Col xsOffset={1} xs={11} smOffset={0} sm={6} className='button-column'>
                                                <Button block bsSize='large' bsStyle={bs.timeoutType === 2 ? 'primary' : 'default'}
                                                    disabled={right.timeOutsRemaining < 1}
                                                    onClick={() => { props.setTimeoutType(bs.boutId, 2) }}>Team Timeout Right</Button>
                                            </Col>
                                        </Row>
                                        <Row>
                                            <Col xs={11} sm={6} className='button-column'>
                                                <Button block bsSize='large' bsStyle={bs.timeoutType === 3 ? 'primary' : 'default'}
                                                    disabled={left.officialReviews < 1}
                                                    onClick={() => { props.setTimeoutType(bs.boutId, 3) }}>Official Review Left</Button>
                                            </Col>
                                            <Col xsOffset={1} xs={11} smOffset={0} sm={6} className='button-column'>
                                                <Button block bsSize='large' bsStyle={bs.timeoutType === 4 ? 'primary' : 'default'}
                                                    disabled={right.officialReviews < 1}
                                                    onClick={() => { props.setTimeoutType(bs.boutId, 4) }}>Official Review Right</Button>
                                            </Col>
                                        </Row>
                                        {
                                            bs.timeoutType === 3 || bs.timeoutType === 4 ? <div>
                                                <h2>Review Kept?</h2>
                                                <Row>
                                                    <Col sm={6} className='button-column'>
                                                        <Button block bsSize='large' bsStyle={!bs.loseOfficialReview ? 'primary' : 'default'} onClick={() => { props.setLoseReview(bs.boutId, false) }}>Keep Review</Button>
                                                    </Col>
                                                    <Col sm={6} className='button-column'>
                                                        <Button block bsSize='large' bsStyle={bs.loseOfficialReview ? 'primary' : 'default'} onClick={() => { props.setLoseReview(bs.boutId, true) }}>Lose Review</Button>
                                                    </Col>
                                                </Row>
                                            </div> : null
                                        }
                                        <hr />
                                        <Row>
                                            <Col sm={12} mdOffset={3} md={6} lgOffset={4} lg={4} className='button-column'>
                                                <Button block bsSize='large' onClick={() => { props.stopTimeout(bs.boutId) }}>End Timeout</Button>
                                            </Col>
                                        </Row>
                                    </div>
                                    : null
                                }
                            </Col>
                        </Row>
                    </Panel.Body></Panel>
                </Col>
                <Col xs={2} sm={1}>
                    <div className='timeout'><Circle className='timeout-circle' color={rto >= 3 ? rcolor : used} /></div>
                    <div className='timeout'><Circle className='timeout-circle' color={rto >= 2 ? rcolor : used} /></div>
                    <div className='timeout'><Circle className='timeout-circle' color={rto >= 1 ? rcolor : used} /></div>
                    <div className='timeout official-review'><Circle className='timeout-circle' color={bs.rightTeamState.officialReviews > 0 ? rcolor : used} /></div>
                </Col>
            </Row>




        </div >)
    }
}

const mapStateToProps = state => {
    return {
        system: state.system,
        boutState: state.boutState
    }
}

const mapDispatchToProps = dispatch => {
    return {
        go: () => dispatch(system.changeScreen('bout')),
        exitPregame: (boutId) => dispatch(jamTimer.exitPregame(boutId)),
        startJam: (boutId) => dispatch(jamTimer.startJam(boutId)),
        stopJam: (boutId) => dispatch(jamTimer.stopJam(boutId)),
        startTimeout: (boutId) => dispatch(jamTimer.startTimeout(boutId)),
        stopTimeout: (boutId) => dispatch(jamTimer.stopTimeout(boutId)),
        setTimeoutType: (boutId, ttype) => dispatch(jamTimer.setTimeoutType(boutId, ttype)),
        setLoseReview: (boutId, lose) => dispatch(jamTimer.setLoseReview(boutId, lose)),
        endPeriod: (boutId) => dispatch(jamTimer.endPeriod(boutId))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(JamTimer);
