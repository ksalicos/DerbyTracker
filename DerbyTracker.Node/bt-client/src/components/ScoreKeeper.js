import React from 'react'
import { connect } from 'react-redux'
import GameSummary from './shared/GameSummary';
import { Row, Col, Button, ButtonGroup, Panel } from 'react-bootstrap'
import TeamScoring from './scoreKeeper/TeamScoring'
import JamSelector from './shared/JamSelector'
import './ScoreKeeper.css'

class ScoreKeeper extends React.Component {
    constructor(props) {
        super(props)

        this.state = {
            viewTeam: 'both',
            jamIndex: props.boutState ? props.boutState.current.jams.length - 1 : null
        }
    }

    render() {
        let bs = this.props.boutState.current
        let data = this.props.boutState.data

        return (<div>
            <GameSummary />

            <JamSelector setJam={(j) => this.setState({ jamIndex: j })} currentIdx={this.state.jamIndex}
                jams={bs.jams} />

            <Panel>
                <Panel.Body>
                    <label className='view-teams-label'>View Team(s)</label>
                    <ButtonGroup>
                        <Button bsStyle={this.state.viewTeam === 'left' ? 'primary' : 'default'}
                            onClick={() => this.setState({ viewTeam: 'left' })}>{data['left'].name}</Button>
                        <Button bsStyle={this.state.viewTeam === 'both' ? 'primary' : 'default'}
                            onClick={() => this.setState({ viewTeam: 'both' })}>Both</Button>
                        <Button bsStyle={this.state.viewTeam === 'right' ? 'primary' : 'default'}
                            onClick={() => this.setState({ viewTeam: 'right' })}>{data['right'].name}</Button>
                    </ButtonGroup>
                </Panel.Body>
            </Panel>

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
