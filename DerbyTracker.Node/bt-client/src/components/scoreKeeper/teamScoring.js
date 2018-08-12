import React from 'react'
import { connect } from 'react-redux'
import { actionCreators as scoreKeeper } from '../../store/scoreKeeperSignalR'
import { Row, Col, Button, Modal, ButtonGroup } from 'react-bootstrap'
import ShadowedPanel from '../shared/ShadowedPanel'
import computed from '../../Computed'

class TeamScoring extends React.Component {
    constructor(props) {
        super(props)

        this.state = {
            showScoreModal: false,
            scoringPass: null
        }
    }

    render() {
        let bs = this.props.boutState.current
        let color = this.props.boutState.data[this.props.team].color
        let currentJam = bs.jams[this.props.jamIndex]
        let currentTeam = currentJam[this.props.team]
        let passes = currentTeam.passes
        let jammerStatus = currentTeam.jammerStatus

        let otherJammerStatus, roster

        if (this.props.team === 'left') {
            otherJammerStatus = currentJam.right.jammerStatus
            roster = this.props.boutState.data['left'].roster
        } else {
            otherJammerStatus = currentJam.left.jammerStatus
            roster = this.props.boutState.data['right'].roster
        }

        let jammer = currentTeam.roster.find(e => e.position === 1)
        let jammerName = jammer ? roster.find(e => e.number === jammer.number).name : 'Jammer'
        let pivot = currentTeam.roster.find(e => e.position === 2)
        let pivotName = pivot ? roster.find(e => e.number === pivot.number).name : 'Pivot'

        let alreadyStarPassed = passes.some(e => e.starPass === true)

        return (<ShadowedPanel color={color}>
            <div className='scorekeeper-label'>
                {alreadyStarPassed
                    ? pivot
                        ? `${pivot.number}: ${pivotName}`
                        : pivotName
                    : jammer
                        ? `${jammer.number}: ${jammerName}`
                        : jammerName}
                < div className='scorekeeper-jamscore'>{computed(bs).jamscores[this.props.jamIndex][this.props.team]}</div>
            </div>
            <Row>
                {
                    otherJammerStatus === 1 || otherJammerStatus === 3
                        ? <Col sm={4}>
                            <Button bsStyle={jammerStatus === 2 ? 'success' : 'default'}
                                onClick={() => this.updateJammerStatus(2)}
                                block>Not Lead</Button>
                        </Col>
                        : <Col sm={4}>
                            <Button bsStyle={jammerStatus === 1 ? 'success' : 'default'}
                                onClick={() => this.updateJammerStatus(1)}
                                block>Lead</Button>
                        </Col>
                }
                <Col sm={4}>
                    <Button bsStyle={jammerStatus === 3 ? 'success' : 'default'}
                        onClick={() => this.updateJammerStatus(3)}
                        block>Lost Lead</Button>
                </Col>
                <Col sm={4}>
                    <Button bsStyle={jammerStatus === 4 ? 'success' : 'default'}
                        onClick={() => { if (jammerStatus === 4) { this.updateJammerStatus(0) } else { this.updateJammerStatus(4) } }}
                        block>Can't Lead</Button>
                </Col>
            </Row>

            <div className='scorekeeper-label'>Passes</div>
            {
                passes.map((e, i) => <Row key={i}>
                    <Col sm={8}>
                        {e.number === 0 ? 'Initial Pass'
                            : <Button onClick={() => this.setState({ showScoreModal: true, scoringPass: e.number, starPass: e.starPass })}
                                block>Score: {e.score}</Button>}

                    </Col>
                    <Col sm={4}>
                        <Button disabled={alreadyStarPassed && !e.starPass}
                            bsStyle={e.starPass ? 'success' : 'default'}
                            onClick={() => this.updatePass(e.number, e.score, !e.starPass)}
                            block>Star Pass</Button>
                    </Col>
                </Row>)
            }
            <div>
                <Button onClick={() => this.createPass()}>Add Pass</Button>
            </div>

            <Modal bsSize="small" show={this.state.showScoreModal} animation={false}>
                <Modal.Header closeButton onHide={() => this.setState({ showScoreModal: false })}>
                    <Modal.Title>Score</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <ButtonGroup bsSize="large" vertical block>
                        {
                            Array(7).fill().map((x, i) =>
                                <Button key={i} onClick={() => { this.updatePass(this.state.scoringPass, i, this.state.starPass) }}>{i}</Button>
                            )
                        }
                    </ButtonGroup>
                </Modal.Body>
            </Modal>
        </ShadowedPanel >)
    }
    updateJammerStatus(newStatus) {
        let bs = this.props.boutState.current
        let currentJam = bs.jams[this.props.jamIndex]
        this.props.updateJammerStatus(bs.boutId, currentJam.period, currentJam.jamNumber, this.props.team, newStatus)
    }
    createPass() {
        let bs = this.props.boutState.current
        let currentJam = bs.jams[this.props.jamIndex]
        this.props.createPass(bs.boutId, currentJam.period, currentJam.jamNumber, this.props.team)
    }
    updatePass(number, score, starPass) {
        let bs = this.props.boutState.current
        let currentJam = bs.jams[this.props.jamIndex]
        this.props.updatePass(bs.boutId, currentJam.period, currentJam.jamNumber, this.props.team,
            { number: number, score: score, starPass: starPass })
        this.setState({ showScoreModal: false });
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
        updateJammerStatus: (boutId, period, jam, team, status) => dispatch(scoreKeeper.updateJammerStatus(boutId, period, jam, team, status)),
        createPass: (boutId, period, jam, team) => dispatch(scoreKeeper.createPass(boutId, period, jam, team)),
        updatePass: (boutId, period, jam, team, pass) => dispatch(scoreKeeper.updatePass(boutId, period, jam, team, pass))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(TeamScoring);
