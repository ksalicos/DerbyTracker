import React from 'react'
import { connect } from 'react-redux'
import { actionCreators as penaltyTracker } from '../store/penaltyTrackerSignalR'
import ShortClockDisplay from './shared/ShortClockDisplay'
import ShortScoreDisplay from './shared/ShortScoreDisplay'
import SkaterSelect from './shared/SkaterSelect'
import { Row, Col, Button, Modal, Table } from 'react-bootstrap'
import penaltyList from '../penalties'

class PenaltyTracker extends React.Component {
    constructor(props) {
        super(props)

        this.state = {
            penaltyId: null,
            viewTeam: 'left',
            showPlayerSelect: false,
            showPenaltySelect: false,
            penaltyCode: null,
            jamIndex: props.boutState ? props.boutState.current.jams.length - 1 : null
        }

        this.lastJam = this.lastJam.bind(this);
        this.nextJam = this.nextJam.bind(this);
        this.createPenalty = this.createPenalty.bind(this);
        this.selectSkater = this.selectSkater.bind(this);
        this.selectPenalty = this.selectPenalty.bind(this);
        this.showSkaterSelect = this.showSkaterSelect.bind(this);
        this.showPenaltySelect = this.showPenaltySelect.bind(this);
    }
    lastJam() {
        if (this.state.jamIndex > 0)
            this.setState({ jamIndex: this.state.jamIndex - 1 })
    }
    nextJam() {
        if (this.state.jamIndex < this.props.boutState.current.jams.length - 1)
            this.setState({ jamIndex: this.state.jamIndex + 1 })
    }
    createPenalty(team) {
        let currentJam = this.props.boutState.current.jams[this.state.jamIndex]
        this.props.createPenalty(this.props.boutState.current.boutId, currentJam.period, currentJam.jamNumber, team)
    }
    showSkaterSelect(id, team) {
        this.setState({ penaltyId: id, viewTeam: team, showPlayerSelect: true })
    }
    showPenaltySelect(id) {
        this.setState({ penaltyId: id, showPenaltySelect: true, penaltyCode: null })
    }
    selectSkater(number) {
        let penalty = this.props.boutState.current.penalties.find(e => e.id === this.state.penaltyId)
        penalty = { ...penalty, number: number }
        this.props.updatePenalty(this.props.boutState.current.boutId, penalty)
        this.setState({
            showPlayerSelect: false
        })
    }
    selectPenalty(code) {
        let penalty = this.props.boutState.current.penalties.find(e => e.id === this.state.penaltyId)
        penalty = { ...penalty, penaltyCode: code }
        this.props.updatePenalty(this.props.boutState.current.boutId, penalty)
        this.setState({
            showPenaltySelect: false
        })
    }

    render() {
        let sort = (a, b) => a.number + '' < b.number + '' ? -1 : 1
        let bs = this.props.boutState.current
        let data = this.props.boutState.data
        let team = data[this.state.viewTeam]
        team.roster.sort(sort)

        let currentJam = bs.jams[this.state.jamIndex]
        let lineup = (this.state.viewTeam === 'left'
            ? currentJam.left.roster
            : currentJam.right.roster)
            .sort(sort)
        let penalties = bs.penalties.filter((e) => { return e.period === currentJam.period && e.jamNumber === currentJam.jamNumber })
            .sort((a, b) => a < b ? -1 : 1)

        let penaltyDisplay = (e, i) => {
            let pteam = data[e.team]
            let skater = pteam.roster.find(r => r.number === e.number)
            skater = skater || { number: -1, name: 'Not Set' }
            return (<Row key={i}>
                <Col sm={6}>
                    <Button block onClick={() => this.showSkaterSelect(e.id, e.team)}>
                        {e.number === -1 ? 'Select Skater' : `${e.number}: ${skater.name}`}
                    </Button>
                </Col>
                <Col sm={6}>
                    <Button block onClick={() => { this.showPenaltySelect(e.id) }}>
                        {e.penaltyCode
                            ? `${e.penaltyCode}: ${penaltyList[e.penaltyCode]}`
                            : 'Select Penalty'}
                    </Button>
                </Col>
            </Row>)
        }

        return (<div>
            <h1>Penalty Tracker</h1>
            <ShortClockDisplay boutState={bs} />
            <ShortScoreDisplay boutState={bs} />

            <Row>
                <Col sm={6}>
                    <Button block onClick={() => this.createPenalty('left')}>Penalty Left</Button>
                </Col>
                <Col sm={6}>
                    <Button block onClick={() => this.createPenalty('right')}>Penalty Right</Button>
                </Col>
            </Row>

            <h2>
                <Button onClick={this.lastJam} disabled={this.state.jamIndex === 0}>Previous</Button>
                Viewing Period {currentJam.period} Jam {currentJam.jamNumber}
                <Button onClick={this.nextJam} disabled={this.state.jamIndex === bs.jams.length - 1}>Next</Button>
            </h2>

            <Row>
                <Col sm={6}>
                    <h2>{data['left'].name}</h2>
                    {penalties.filter(e => e.team === 'left').map(penaltyDisplay)}
                </Col>
                <Col sm={6}>
                    <h2>{data['right'].name}</h2>
                    {penalties.filter(e => e.team === 'right').map(penaltyDisplay)}
                </Col>
            </Row>

            <div className='bottom'>
                <h2>Penalty Status</h2>
                <Row>
                    <Col sm={6}>
                        <Table responsive>
                            <tbody>
                                {currentJam.left.roster.map((e, i) => {
                                    let skater = data['left'].roster.find(r => r.number === e.number)
                                    let penalties = bs.penalties.filter(p => p.number === e.number && p.team === 'left').length
                                    let color = penalties > 5 ? (penalties >= 7 ? 'penaltyAlert' : 'penaltyWarning') : ''
                                    return (<tr key={i}>
                                        <td>{e.number}</td>
                                        <td>{skater.name}</td>
                                        <td className={color}>{penalties}</td>
                                    </tr>)
                                })}
                            </tbody>
                        </Table>
                    </Col>
                    <Col sm={6}>
                        <Table responsive>
                            <tbody>
                                {currentJam.right.roster.map((e, i) => {
                                    let skater = data['right'].roster.find(r => r.number === e.number)
                                    let penalties = bs.penalties.filter(p => p.number === e.number && p.team === 'right').length
                                    let color = penalties > 5 ? (penalties >= 7 ? 'penaltyAlert' : 'penaltyWarning') : ''
                                    return (<tr key={i}>
                                        <td>{e.number}</td>
                                        <td>{skater.name}</td>
                                        <td className={color}>{penalties}</td>
                                    </tr>)
                                })}
                            </tbody>
                        </Table>
                    </Col>
                </Row>
            </div>

            <SkaterSelect show={this.state.showPlayerSelect} close={() => this.setState({ showPlayerSelect: false })}
                selectSkater={this.selectSkater} roster={team.roster} lineup={lineup} />

            <Modal show={this.state.showPenaltySelect} animation={false}>
                <Modal.Header closeButton onHide={() => this.setState({ showPenaltySelect: false })}>
                    <Modal.Title>Select Penalty</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Row>
                        {
                            Object.keys(penaltyList).map((e, i) =>
                                <Col sm={6} key={i}>
                                    <Row>
                                        <Col sm={4}>
                                            <Button block onClick={() => this.selectPenalty(e)}>{e}</Button>
                                        </Col>
                                        <Col sm={8}>
                                            {penaltyList[e]}
                                        </Col>
                                    </Row>
                                </Col>
                            )
                        }
                    </Row>
                </Modal.Body>
            </Modal>
        </div>)
    }
}

const mapStateToProps = state => {
    return {
        system: state.system,
        boutState: state.boutState,
    }
}

const mapDispatchToProps = dispatch => {
    return {
        createPenalty: (boutId, period, jam, team) => dispatch(penaltyTracker.createPenalty(boutId, period, jam, team)),
        updatePenalty: (boutId, penalty) => dispatch(penaltyTracker.updatePenalty(boutId, penalty))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(PenaltyTracker);
