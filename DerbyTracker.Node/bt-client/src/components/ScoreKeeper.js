import React from 'react'
import { connect } from 'react-redux'
import ShortClockDisplay from './shared/ShortClockDisplay';
import ShortScoreDisplay from './shared/ShortScoreDisplay';
import { Row, Col, Button } from 'react-bootstrap'
import TeamScoring from './scoreKeeper/teamScoring'

class ScoreKeeper extends React.Component {
    constructor(props) {
        super(props)

        this.state = {
            viewTeam: 'both',
            jamIndex: props.boutState ? props.boutState.current.jams.length - 1 : null
        }
        this.lastJam = this.lastJam.bind(this);
        this.nextJam = this.nextJam.bind(this);
    }
    lastJam() {
        if (this.state.jamIndex > 0)
            this.setState({ jamIndex: this.state.jamIndex - 1 })
    }
    nextJam() {
        if (this.state.jamIndex < this.props.boutState.current.jams.length - 1)
            this.setState({ jamIndex: this.state.jamIndex + 1 })
    }
    render() {
        let bs = this.props.boutState.current
        let currentJam = bs.jams[this.state.jamIndex]

        return (<div>
            <h1>Score Keeper</h1>
            <ShortClockDisplay boutState={bs} />
            <ShortScoreDisplay boutState={bs} />

            <h2>
                <Button onClick={this.lastJam} disabled={this.state.jamIndex === 0}>Previous</Button>
                Viewing Period {currentJam.period} Jam {currentJam.jamNumber}
                <Button onClick={this.nextJam} disabled={this.state.jamIndex === bs.jams.length - 1}>Next</Button>
            </h2>

            <h1>View Team(s)</h1>
            <Row>
                <Col sm={4}>
                    <Button block onClick={() => this.setState({ viewTeam: 'left' })}>Left</Button>
                </Col>
                <Col sm={4}>
                    <Button block onClick={() => this.setState({ viewTeam: 'both' })}>Both</Button>
                </Col>
                <Col sm={4}>
                    <Button block onClick={() => this.setState({ viewTeam: 'right' })}>Right</Button>
                </Col>
            </Row>

            {this.state.viewTeam === 'both'
                ? <Row>
                    <Col sm={6}>
                        <TeamScoring team='left' jamIndex={this.state.jamIndex} />
                    </Col>
                    <Col sm={6}>
                        <TeamScoring team='right' jamIndex={this.state.jamIndex} />
                    </Col>
                </Row>
                :
                <Row>
                    <Col sm={6} smOffset={3}>
                        <TeamScoring team={this.state.viewTeam} jamIndex={this.state.jamIndex} />
                    </Col>
                </Row>
            }
        </div>)
    }
}

const mapStateToProps = state => {
    return {
        boutState: state.boutState
    }
}

export default connect(mapStateToProps)(ScoreKeeper);
