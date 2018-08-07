import React from 'react'
import { connect } from 'react-redux'
import { actionCreators as penaltyTracker } from '../store/penaltyTrackerSignalR'
import ShortClockDisplay from './shared/ShortClockDisplay';
import ShortScoreDisplay from './shared/ShortScoreDisplay';
import { Row, Col, Button, Modal } from 'react-bootstrap'
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
            viewPeriod: 1,
            viewJam: 1,
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
        this.props.createPenalty(this.props.boutState.current.boutId, this.state.viewPeriod, this.state.viewJam, team)
    }
    showSkaterSelect(id, team) {
        console.log('show skater', id, team)
        this.setState({ penaltyId: id, viewTeam: team, showPlayerSelect: true })
    }
    showPenaltySelect(id) {
        this.setState({ penaltyId: id, showPenaltySelect: true, penaltyCode: null })
    }
    selectSkater(number) {
        let penalty = this.props.boutState.current.penalties.find(e => e.id === this.state.penaltyId)
        console.log(penalty)
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

            {penalties.map((e, i) => {
                let pteam = data[e.team]
                let skater = pteam.roster.find(r => r.number === e.number)
                skater = skater || { number: -1, name: 'Not Set' }
                return (<Row key={i}>
                    <Col sm={2}>
                        <Button block onClick={() => this.showSkaterSelect(e.id, e.team)}>
                            {e.number === -1 ? 'Select Skater' : e.number}
                        </Button>
                    </Col>
                    <Col sm={2}>{skater.name}</Col>
                    <Col sm={2}>{pteam.name}</Col>
                    <Col sm={2}>
                        <Button block onClick={() => { this.showPenaltySelect(e.id) }}>
                            {e.penaltyCode || "Select Penalty"}
                        </Button>
                    </Col>
                    <Col sm={2}>{e.penaltyCode ? penaltyList[e.penaltyCode] : null}</Col>
                </Row>)
            })}

            <Modal show={this.state.showPlayerSelect} animation={false}>
                <Modal.Header closeButton onHide={() => this.setState({ showPlayerSelect: false })}>
                    <Modal.Title>Select Skater</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <h3>Current Lineup</h3>
                    <Row>
                        {lineup.map(e => {
                            return (<Col sm={2} key={e.number}>
                                <Button block onClick={() => this.selectSkater(e.number)}>{e.number}</Button>
                            </Col>)
                        })}
                    </Row>
                    <h3>Full Roster</h3>
                    <Row>
                        {team.roster.map(e => {
                            return (<Col sm={2} key={e.number}>
                                <Button block onClick={() => this.selectSkater(e.number)}>{e.number}</Button>
                            </Col>)
                        })}
                    </Row>
                </Modal.Body>
            </Modal>

            <Modal show={this.state.showPenaltySelect} animation={false}>
                <Modal.Header closeButton onHide={() => this.setState({ showPenaltySelect: false })}>
                    <Modal.Title>Select Penalty</Modal.Title>
                </Modal.Header>
                <Row>
                    <Modal.Body>
                        {

                            Object.keys(penaltyList).map((e, i) =>
                                <Col sm={2} key={i}>
                                    <Button block onClick={() => this.selectPenalty(e)}>{e}</Button>
                                </Col>
                            )
                        }
                    </Modal.Body>
                </Row>
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
