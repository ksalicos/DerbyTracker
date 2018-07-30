
import React from 'react'
import { connect } from 'react-redux'
import { actionCreators as lineupsTracker } from '../store/lineupsTrackerSignalR'
import ShortClockDisplay from './shared/ShortClockDisplay';
import ShortScoreDisplay from './shared/ShortScoreDisplay';
import { Row, Col, Button, ButtonGroup, Glyphicon } from 'react-bootstrap'

class LineupsTracker extends React.Component {
    constructor(props) {
        super(props)

        this.state = {
            viewTeam: 'left',
            viewPeriod: 1,
            viewJam: 1,
            jamIndex: props.boutState ? props.boutState.current.jams.length - 1 : null
        }

        this.lastJam = this.lastJam.bind(this);
        this.nextJam = this.nextJam.bind(this);
    }

    render() {
        let sort = (a, b) => a.number < b.number ? -1 : 1
        let bs = this.props.boutState.current
        let data = this.props.boutState.data
        let team = this.state.viewTeam === 'left'
            ? data.leftTeam
            : data.rightTeam
        team.roster.sort(sort)
        let currentJam = bs.jams[this.state.jamIndex]
        let lineup = currentJam[this.state.viewTeam].roster
            .sort(sort)

        return (<div>
            <h1>Lineups Tracker</h1>
            <ShortClockDisplay boutState={bs} />
            <ShortScoreDisplay boutState={bs} />

            <h2>
                <Button onClick={this.lastJam} disabled={this.state.jamIndex === 0}>Previous</Button>
                Viewing Period {currentJam.period} Jam {currentJam.jamNumber}
                <Button onClick={this.nextJam} disabled={this.state.jamIndex === bs.jams.length - 1}>Next</Button>
            </h2>
            <Row>
                {lineup.map((e, i) => {
                    let name = team.roster.find((r) => { return e.number === r.number }).name
                    return (<Col sm={6} key={i}>
                        <Row className='lineups-skater'>
                            <Col sm={2} className='lineups-number'>
                                <Button onClick={() => {
                                    this.props.removeSkater(bs.boutId, currentJam.period, currentJam.jamNumber,
                                        this.state.viewTeam, e.number)
                                }}
                                    bsStyle='success' bsSize="large" block>{e.number}</Button>
                            </Col>
                            <Col sm={4} className='lineups-name'>{name}</Col>
                            <Col sm={2}>
                                <Button bsStyle={e.position === 0 ? 'success' : 'default'} onClick={() => {
                                    this.props.setSkaterPosition(bs.boutId, currentJam.period, currentJam.jamNumber,
                                        this.state.viewTeam, e.number, 0)
                                }} bsSize="large" block><Glyphicon glyph='bold' /></Button>
                            </Col>
                            <Col sm={2}>
                                <Button bsStyle={e.position === 1 ? 'success' : 'default'} onClick={() => {
                                    this.props.setSkaterPosition(bs.boutId, currentJam.period, currentJam.jamNumber,
                                        this.state.viewTeam, e.number, 1)
                                }} bsSize="large" block><Glyphicon glyph='star' /></Button>
                            </Col>
                            <Col sm={2}>
                                <Button bsStyle={e.position === 2 ? 'success' : 'default'} onClick={() => {
                                    this.props.setSkaterPosition(bs.boutId, currentJam.period, currentJam.jamNumber,
                                        this.state.viewTeam, e.number, 2)
                                }} bsSize="large" block><Glyphicon glyph='stop' /></Button>
                            </Col>
                        </Row>
                    </Col>)
                })}
            </Row>

            <h2>
                <ButtonGroup bsSize="large">
                    <Button bsStyle={this.state.viewTeam === 'left' ? 'primary' : 'default'}
                        onClick={() => { this.setState({ viewTeam: 'left' }) }}>
                        {data.leftTeam.name}
                    </Button>
                    <Button bsStyle={this.state.viewTeam === 'right' ? 'primary' : 'default'}
                        onClick={() => { this.setState({ viewTeam: 'right' }) }}>
                        {data.rightTeam.name}
                    </Button>
                </ButtonGroup>
            </h2>
            <Row>
                {
                    team.roster.map((e, i) => {
                        let buttonStyle = 'primary' //Set this when penalties are tracked.
                        if (lineup.find(r => r.number === e.number)) {
                            buttonStyle = 'success'
                        }
                        //Red: In box but not jam
                        //Yellow: In box and jam?
                        //Green: In jam
                        //Blue: Sad, give her a hug

                        return (
                            <Col sm={6} key={i}>
                                <Row className='lineups-skater'>
                                    <Col sm={4} className='lineups-number'>
                                        <Button onClick={() => {
                                            this.props.addSkater(bs.boutId, currentJam.period, currentJam.jamNumber,
                                                this.state.viewTeam, e.number)
                                        }}
                                            bsStyle={buttonStyle} bsSize="large" block>{e.number}</Button>
                                    </Col>
                                    <Col sm={8} className='lineups-name'>{e.name}</Col>
                                </Row>
                            </Col>)
                    })
                }
            </Row>

        </div>)
    }

    lastJam() {
        if (this.state.jamIndex > 0)
            this.setState({ jamIndex: this.state.jamIndex - 1 })
    }
    nextJam() {
        if (this.state.jamIndex < this.props.boutState.current.jams.length - 1)
            this.setState({ jamIndex: this.state.jamIndex + 1 })
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
        addSkater: (boutId, period, jam, team, number) => dispatch(lineupsTracker.addSkater(boutId, period, jam, team, number)),
        removeSkater: (boutId, period, jam, team, number) => dispatch(lineupsTracker.removeSkater(boutId, period, jam, team, number)),
        setSkaterPosition: (boutId, period, jam, team, number, position) => dispatch(lineupsTracker.setSkaterPosition(boutId, period, jam, team, number, position))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(LineupsTracker);
