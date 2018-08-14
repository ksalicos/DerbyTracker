import React from 'react'
import { connect } from 'react-redux'
import GameSummary from './shared/GameSummary';
import { Row, Col, Button, Panel, Well, Tabs, Tab } from 'react-bootstrap'
import { actionCreators as penaltyTimer } from '../store/penaltyBoxTimerSignalR'
import uuid from 'uuid'
import moment from 'moment'
import TimeDisplay from './shared/TimeDisplay'
import * as clock from '../clocks'
import SkaterSelect from './shared/SkaterSelect'
import './PenaltyBoxTimer.css'
import Queue from './PenaltyBoxTimer/Queue'
import computed from '../Computed'

class PenaltyBoxTimer extends React.Component {
    constructor(props) {
        super(props)

        this.state = {
            watchTeam: 'jammers',
            currentTime: moment(),
            showSelect: null,
            id: null
        }
        this.openSelect = this.openSelect.bind(this);
        this.setSkaterNumber = this.setSkaterNumber.bind(this);
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

        let queue = computed(bs).queue

        let chair = (team, chairNumber, isJammer) => {
            let penalty = bs.penaltyBox.find(x => x.team === team && x.chairNumber === chairNumber && x.isJammer === isJammer)
            if (!penalty) return <Well bsSize='small'>
                {isJammer
                    ? <div className='penalty-time'>{data[team].name}</div>
                    : null}
                <div><Button block bsSize='large' onClick={() => { this.props.buttHitSeat(bs.boutId, team, chairNumber, isJammer) }}>Butt In Seat</Button></div>
            </Well>

            let roster = data[team].roster.sort(sort)
            let time = Math.max(penalty.secondsOwed * 1000 - (penalty.stopWatch.running
                ? (this.state.currentTime.diff(penalty.stopWatch.lastStarted) + penalty.stopWatch.elapsedMs)
                : penalty.stopWatch.elapsedMs), 0)

            return (<Well bsSize='small'>
                {isJammer
                    ? <div className='penalty-time'>{data[team].name}</div>
                    : null}
                <div>
                    {
                        penalty.number === -1
                            ? <div><Button block bsSize='large' onClick={() => this.openSelect(penalty.id, penalty.team, isJammer)}>Set Skater</Button></div>
                            : <div><Button block bsSize='large' onClick={() => this.openSelect(penalty.id, penalty.team, isJammer)}>
                                {penalty.number}: {roster.find((r) => { return penalty.number === r.number }).name}
                            </Button></div>
                    }
                </div>
                <div className={time < 10000 ? 'stand penalty-time' : 'penalty-time'}><TimeDisplay ms={time} /></div>
                <div><Button block bsSize='large' onClick={() => { this.props.releaseSkater(bs.boutId, penalty.id) }}>Release</Button></div>
                <Row>
                    <Col sm={6}>
                        <Button block>Left Box</Button>
                        <Button block>Stop Clock</Button>
                        <Button block onClick={() => this.props.cancelSit(bs.boutId, penalty.id)}>No Sit</Button>
                    </Col>
                    <Col sm={6}>
                        <Button block disabled={penalty.number !== -1}
                            onClick={() => this.props.updateChair(bs.boutId, { ...penalty, secondsOwed: penalty.secondsOwed + 30 })}>
                            Add 30s</Button>
                        <Button block disabled={penalty.secondsOwed <= 30}
                            onClick={() => this.props.updateChair(bs.boutId, { ...penalty, secondsOwed: penalty.secondsOwed - 30 })}>
                            Remove 30s</Button>
                    </Col>
                </Row>
                <div></div>
                <div></div>
            </Well>)
        }

        return (<div>
            <GameSummary />

            <Panel><Panel.Body>

                <Tabs defaultActiveKey={3} id="uncontrolled-tab-example">
                    <Tab eventKey={1} title={data['left'].name}>
                        <h1>{data['left'].name} Blockers</h1>
                        <Row>
                            <Col sm={4}>
                                {chair('left', 1, false)}
                            </Col>
                            <Col sm={4}>
                                {chair('left', 2, false)}
                            </Col>
                            <Col sm={4}>
                                {chair('left', 3, false)}
                            </Col>
                        </Row>
                        <Queue penalties={queue.filter(q => q.seconds > 0 && q.team === 'left' && leftLineup.some(r => q.number === r.number && r.position !== 1))} />
                    </Tab>
                    <Tab eventKey={2} title={data['right'].name}>
                        <h1>{data['right'].name} Blockers</h1>
                        <Row>
                            <Col sm={4}>
                                {chair('right', 1, false)}
                            </Col>
                            <Col sm={4}>
                                {chair('right', 2, false)}
                            </Col>
                            <Col sm={4}>
                                {chair('right', 3, false)}
                            </Col>
                        </Row>
                        <Queue penalties={queue.filter(q => q.seconds > 0 && q.team === 'right' && rightLineup.some(r => q.number === r.number && r.position !== 1))} />
                    </Tab>
                    <Tab eventKey={3} title="Jammers">
                        <h1>Jammers</h1>
                        <Row>
                            <Col sm={6}>
                                {chair('left', 1, true)}
                            </Col>
                            <Col sm={6}>
                                {chair('right', 2, true)}
                            </Col>
                        </Row>
                        <Queue penalties={queue.filter(q => q.seconds > 0
                            && (leftLineup.some(r => q.number === r.number && r.position === 1) || rightLineup.some(r => q.number === r.number && r.position === 1)))} />
                    </Tab>
                </Tabs>
            </Panel.Body></Panel>

            <SkaterSelect show={this.state.showSelect === 'left'} close={() => this.setState({ showSelect: '' })}
                selectSkater={this.setSkaterNumber} roster={leftRoster} lineup={leftLineup} />
            <SkaterSelect show={this.state.showSelect === 'right'} close={() => this.setState({ showSelect: '' })}
                selectSkater={this.setSkaterNumber} roster={rightRoster} lineup={rightLineup} />
        </div>)
    }
    openSelect(id, team) {
        this.setState({ id: id, showSelect: team })
    }
    setSkaterNumber(n) {
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
        buttHitSeat: (boutId, team, chairNumber, isJammer) => dispatch(penaltyTimer.buttHitSeat(boutId, { id: uuid.v4(), team: team, isJammer: isJammer, number: -1, chairNumber: chairNumber })),
        updateChair: (boutId, chair) => dispatch(penaltyTimer.updateChair(boutId, chair)),
        releaseSkater: (boutId, id) => dispatch(penaltyTimer.releaseSkater(boutId, id)),
        cancelSit: (boutId, id) => dispatch(penaltyTimer.cancelSit(boutId, id))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(PenaltyBoxTimer);
