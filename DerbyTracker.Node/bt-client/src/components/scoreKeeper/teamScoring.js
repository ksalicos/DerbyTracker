import React from 'react'
import { connect } from 'react-redux'
import { actionCreators as scoreKeeper } from '../../store/scoreKeeperSignalR'
import { Row, Col, Button, Modal } from 'react-bootstrap'

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
        let currentJam = bs.jams[this.props.jamIndex]
        let currentTeam = currentJam[this.props.team]
        let passes = currentTeam.passes
        let jammerStatus = currentTeam.jammerStatus

        let otherJammerStatus, roster

        if (this.props.team === 'left') {
            otherJammerStatus = currentJam.right.jammerStatus
            roster = this.props.boutState.data.leftTeam.roster
        } else {
            otherJammerStatus = currentJam.left.jammerStatus
            roster = this.props.boutState.data.rightTeam.roster
        }

        let jammer = currentTeam.roster.find(e => e.position === 1)
        let jammerName = jammer ? roster.find(e => e.number === jammer.number).name : 'Jammer'
        let pivot = currentTeam.roster.find(e => e.position === 2)
        let pivotName = pivot ? roster.find(e => e.number === pivot.number).name : 'Pivot'

        let alreadyStarPassed = passes.some(e => e.starPass === true)


        //Can't become lead if other jammer is
        //Cant become lead if lost
        //Cant before lead if can't become lead
        //Cant lose lead if not lead

        return (<div>
            <h1>{alreadyStarPassed ? `${pivotName} (from ${jammerName})` : jammerName}</h1>
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
                        onClick={() => this.updateJammerStatus(4)}
                        block>Can't Lead</Button>
                </Col>
            </Row>

            <h1>Passes</h1>
            {passes.map((e, i) => <Row key={i}>
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
            </Row>)}
            <div>
                <Button onClick={() => this.createPass()}>Add Pass</Button>
            </div>

            <Modal show={this.state.showScoreModal} animation={false}>
                <Modal.Header closeButton onHide={() => this.setState({ showScoreModal: false })}>
                    <Modal.Title>Score</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Row>
                        <Col sm={2}>
                            <Button onClick={() => { this.updatePass(this.state.scoringPass, 0, this.state.starPass) }} block>0</Button>
                        </Col>
                        <Col sm={2}>
                            <Button onClick={() => { this.updatePass(this.state.scoringPass, 1, this.state.starPass) }} block>1</Button>
                        </Col>
                        <Col sm={2}>
                            <Button onClick={() => { this.updatePass(this.state.scoringPass, 2, this.state.starPass) }} block>2</Button>
                        </Col>
                        <Col sm={2}>
                            <Button onClick={() => { this.updatePass(this.state.scoringPass, 3, this.state.starPass) }} block>3</Button>
                        </Col>
                        <Col sm={2}>
                            <Button onClick={() => { this.updatePass(this.state.scoringPass, 4, this.state.starPass) }} block>4</Button>
                        </Col>
                        <Col sm={2}>
                            <Button onClick={() => { this.updatePass(this.state.scoringPass, 5, this.state.starPass) }} block>5</Button>
                        </Col>
                        <Col sm={2}>
                            <Button onClick={() => { this.updatePass(this.state.scoringPass, 6, this.state.starPass) }} block>6</Button>
                        </Col>
                    </Row>
                </Modal.Body>
            </Modal>
        </div>)
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
