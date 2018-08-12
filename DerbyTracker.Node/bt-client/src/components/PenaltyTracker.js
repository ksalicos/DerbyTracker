import React from 'react'
import { connect } from 'react-redux'
import { actionCreators as penaltyTracker } from '../store/penaltyTrackerSignalR'
import GameSummary from './shared/GameSummary'
import SkaterSelect from './shared/SkaterSelect'
import { Row, Col, Button, Modal, Table, Navbar } from 'react-bootstrap'
import penaltyList from '../penalties'
import JamSelector from './shared/JamSelector'
import ShadowedPanel from './shared/ShadowedPanel';
import './PenaltyTracker.css'

class PenaltyTracker extends React.Component {
    constructor(props) {
        super(props)

        this.state = {
            penaltyId: null,
            viewTeam: 'left',
            showPlayerSelect: false,
            showPenaltySelect: false,
            penaltyCode: null,
        }

        this.createPenalty = this.createPenalty.bind(this);
        this.selectSkater = this.selectSkater.bind(this);
        this.selectPenalty = this.selectPenalty.bind(this);
        this.showSkaterSelect = this.showSkaterSelect.bind(this);
        this.showPenaltySelect = this.showPenaltySelect.bind(this);
    }

    createPenalty(team) {
        let currentJam = this.props.boutState.current.jams[this.props.jam.index]
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

        let currentJam = bs.jams[this.props.jam.index]

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
            return (
                <Row key={i}>
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
                </Row>
            )
        }

        return (<div>
            <GameSummary />
            <JamSelector />

            <Row>
                <Col sm={6}>
                    <ShadowedPanel color={data['left'].color}>
                        <Button bsSize="large" block onClick={() => this.createPenalty('left')}>Penalty Left</Button>
                        <div className='penalty-team-name'>{data['left'].name}</div>
                        {penalties.filter(e => e.team === 'left').map(penaltyDisplay)}
                    </ShadowedPanel>
                </Col>
                <Col sm={6}>
                    <ShadowedPanel subtle color={data['right'].color}>
                        <Button bsSize="large" block onClick={() => this.createPenalty('right')}>Penalty Right</Button>
                        <div className='penalty-team-name'>{data['right'].name}</div>
                        {penalties.filter(e => e.team === 'right').map(penaltyDisplay)}
                    </ShadowedPanel>
                </Col>
            </Row>

            <Navbar fixedBottom>
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
            </Navbar>

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
        jam: state.currentJam
    }
}

const mapDispatchToProps = dispatch => {
    return {
        createPenalty: (boutId, period, jam, team) => dispatch(penaltyTracker.createPenalty(boutId, period, jam, team)),
        updatePenalty: (boutId, penalty) => dispatch(penaltyTracker.updatePenalty(boutId, penalty))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(PenaltyTracker);
