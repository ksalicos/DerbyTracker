import React from 'react'
import { connect } from 'react-redux'
import ShortClockDisplay from './shared/ShortClockDisplay';
import ShortScoreDisplay from './shared/ShortScoreDisplay';
import { Row, Col, Button } from 'react-bootstrap'
import { actionCreators as penaltyTimer } from '../store/penaltyBoxTimerSignalR'
import uuid from 'uuid'
import moment from 'moment'
import TimeDisplay from './shared/TimeDisplay'
import * as clock from '../clocks'
import SkaterSelect from './shared/SkaterSelect'

class PenaltyBoxTimer extends React.Component {
    constructor(props) {
        super(props)

        this.state = {
            currentTime: moment(),
            showSelect: null,
            id: null
        }
        this.openSelect = this.openSelect.bind(this);
        this.updateChair = this.updateChair.bind(this);
    }

    componentDidMount() {
        clock.addWatch('pbt', (t) => {
            this.setState({
                currentTime: t.now
            })
        })
    }
    componentWillUnmount() {
        clock.removeWatch('pbt')
    }

    render() {
        let sort = (a, b) => a.number + '' < b.number + '' ? -1 : 1

        let bs = this.props.boutState.current
        let data = this.props.boutState.data

        let leftRoster = data['left'].roster.sort(sort)
        let leftLineup = bs.jams[bs.jams.length - 1].left.roster.sort(sort)
        let rightRoster = data['right'].roster.sort(sort)
        let rightLineup = bs.jams[bs.jams.length - 1].right.roster.sort(sort)

        let leftPenalties = bs.penaltyBox.filter(e => e.team === 'left')
        let rightPenalties = bs.penaltyBox.filter(e => e.team === 'right')

        let penaltyMap = (e, i) => {
            let time = Math.max(e.secondsOwed * 1000 - (e.stopWatch.running
                ? (this.state.currentTime.diff(e.stopWatch.lastStarted) + e.stopWatch.elapsedMs)
                : e.stopWatch.elapsedMs), 0)
            let roster = data[e.team].roster.sort(sort)
            console.log(time, e.secondsOwed)
            return (<Col sm={2} key={i}>
                <div>
                    {
                        e.number === -1
                            ? <div><Button onClick={() => this.openSelect(e.id, e.team)}>Set Skater</Button></div>
                            : <div>{e.number}:{roster.find((r) => { return e.number === r.number }).name}</div>
                    }
                </div>
                <div><TimeDisplay ms={time} /></div>
                <div><Button onClick={() => { this.props.releaseSkater(bs.boutId, e.id) }}>Release</Button></div>
            </Col>)
        }

        return (<div>
            <h1>Penalty Box Timer</h1>
            <ShortClockDisplay boutState={bs} />
            <ShortScoreDisplay boutState={bs} />

            <Row>
                <Col sm={6}>
                    <h1>Left Blockers</h1>
                    <div>
                        <Button disabled={leftPenalties.filter(e => !e.isJammer).length >= 2} onClick={() => { this.props.buttHitSeat(bs.boutId, 'left', false) }}>Butt Down</Button>
                    </div>
                </Col>
                <Col sm={2} smOffset={1}>
                    <h1>Jammer</h1>
                    <div>
                        <Button disabled={leftPenalties.filter(e => e.isJammer).length > 0} onClick={() => { this.props.buttHitSeat(bs.boutId, 'left', true) }}>Butt Down</Button>
                    </div>
                </Col>
            </Row>
            <Row>
                {
                    leftPenalties.filter(e => !e.isJammer).map(penaltyMap)
                }
                <Col sm={3} smOffset={6 - leftPenalties.length * 2}></Col>
                {
                    leftPenalties.filter(e => e.isJammer).map(penaltyMap)
                }
            </Row>

            <Row>
                <Col sm={6}>
                    <h1>Right Blockers</h1>
                    <div>
                        <Button disabled={rightPenalties.filter(e => !e.isJammer).length >= 2} onClick={() => { this.props.buttHitSeat(bs.boutId, 'right', false) }}>Butt Down</Button>
                    </div>
                </Col>
                <Col sm={2} smOffset={1}>
                    <h1>Jammer</h1>
                    <div>
                        <Button disabled={rightPenalties.filter(e => e.isJammer).length > 0} onClick={() => { this.props.buttHitSeat(bs.boutId, 'right', true) }}>Butt Down</Button>
                    </div>
                </Col>
            </Row>
            <Row>
                {
                    rightPenalties.filter(e => !e.isJammer).map(penaltyMap)
                }
                <Col sm={3} smOffset={6 - rightPenalties.length * 2}></Col>
                {
                    rightPenalties.filter(e => e.isJammer).map(penaltyMap)
                }
            </Row>

            <SkaterSelect show={this.state.showSelect === 'left'} close={() => this.setState({ showSelect: '' })}
                selectSkater={this.updateChair} roster={leftRoster} lineup={leftLineup} />
            <SkaterSelect show={this.state.showSelect === 'right'} close={() => this.setState({ showSelect: '' })}
                selectSkater={this.updateChair} roster={rightRoster} lineup={rightLineup} />
        </div>)
    }
    openSelect(id, team) {
        this.setState({ id: id, showSelect: team })
    }
    updateChair(n) {
        let chair = this.props.boutState.current.penaltyBox.find(x => x.id === this.state.id)
        if (chair) {
            this.props.updateChair(this.props.boutState.current.boutId, { ...chair, number: n })
        }
        this.setState({ showSelect: '' })
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
        buttHitSeat: (boutId, team, isJammer) => dispatch(penaltyTimer.buttHitSeat(boutId, { id: uuid.v4(), team: team, isJammer: isJammer, number: -1 })),
        updateChair: (boutId, chair) => dispatch(penaltyTimer.updateChair(boutId, chair)),
        releaseSkater: (boutId, id) => dispatch(penaltyTimer.releaseSkater(boutId, id)),
        cancelSit: (boutId, id) => dispatch(penaltyTimer.cancelSit(boutId, id))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(PenaltyBoxTimer);
