
import React from 'react'
import { connect } from 'react-redux'
import { Grid, Row, Col, Image } from 'react-bootstrap'
import * as clock from '../clocks'
import { phaseList } from '../store/BoutState'
import TimeDisplay from './shared/TimeDisplay'

class Scoreboard extends React.Component {
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
        clock.addWatch('scb', (t) => {
            this.setState({
                jamClock: t.jam,
                gameClock: t.game,
                lineupClock: t.lineup
            })
        })
    }
    componentWillUnmount() {
        clock.removeWatch('scb')
    }

    render() {
        let bs = this.props.boutState.current
        let data = this.props.boutState.data

        return (<Grid fluid id='scoreboard-box' className='fill-height'>
            <Row>
                <Col smOffset={1} sm={4}>
                    <div className='team-name'>{data.leftTeam.name}</div>
                </Col>
                <Col smOffset={2} sm={4}>
                    <div className='team-name'>{data.rightTeam.name}</div>
                </Col>
            </Row>
            <Row>
                <Col smOffset={2} sm={2}>
                    <Image alt='logo' src='http://via.placeholder.com/2222' responsive />
                </Col>
                <Col smOffset={4} sm={2}>
                    <Image alt='logo' src='http://via.placeholder.com/2222' responsive />
                </Col>
            </Row>
            <Row>
                <Col sm={1}>
                    <div className='timeout-wrapper'>
                        <div className='timeout'><span role='img' aria-label='dot'>⚫</span></div>
                        <div className='timeout'><span role='img' aria-label='dot'>⚫</span></div>
                        <div className='timeout'><span role='img' aria-label='dot'>⚫</span></div>
                    </div>
                    <div className='timeout-wrapper'>
                        <div className='timeout'><span role='img' aria-label='dot'>⚫</span></div>
                    </div>
                </Col>
                <Col sm={3}>
                    <div className='team-score-wrapper'>
                        <div className='team-score'>{bs.leftTeamState.score}</div>
                    </div>
                </Col>
                <Col sm={2}>
                    <div className='jammer'>Gal Of Fray</div>
                    <div className='jam-score-wrapper'>
                        <div className='jam-score'>14</div>
                    </div>
                </Col>
                <Col sm={2}>
                    <div className='jammer'>Zip Drive</div>
                    <div className='jam-score-wrapper'>
                        <div className='jam-score'>4</div>
                    </div>
                </Col>
                <Col sm={3}>
                    <div className='team-score-wrapper'>
                        <div className='team-score'>{bs.rightTeamState.score}</div>
                    </div>
                </Col>
                <Col sm={1}>
                    <div className='timeout-wrapper'>
                        <div className='timeout'><span role='img' aria-label='dot'>⚫</span></div>
                        <div className='timeout'><span role='img' aria-label='dot'>⚫</span></div>
                        <div className='timeout'><span role='img' aria-label='dot'>⚫</span></div>
                    </div>
                    <div className='timeout-wrapper'>
                        <div className='timeout'><span role='img' aria-label='dot'>⚫</span></div>
                    </div>
                </Col>
            </Row>
            <Row>
                <Col smOffset={1} sm={4}>
                    <div className='stat-box'>
                        <div className='stat-label'>Period {bs.period}</div>
                        <div className='stat-text'>
                            <TimeDisplay ms={this.state.gameClock} />
                        </div>
                    </div>
                </Col>

                <Col smOffset={2} sm={4}>
                    <div className='stat-box'>
                        <div className='stat-label'>{phaseList[bs.phase]} {bs.jam}</div>
                        <div className='stat-text'>
                            <TimeDisplay ms={bs.phase === 1
                                ? this.state.lineupClock
                                : this.state.jamClock}
                            />
                        </div>
                    </div>
                </Col>
            </Row>
        </Grid >)
    }
}

const mapStateToProps = state => {
    return {
        boutState: state.boutState
    }
}

export default connect(mapStateToProps)(Scoreboard);
