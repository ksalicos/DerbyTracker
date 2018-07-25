
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
            viewJam: 1
        }
    }
    render() {
        let bs = this.props.boutState.current
        let data = this.props.boutState.data
        let team = this.state.viewTeam === 'left'
            ? data.leftTeam
            : data.rightTeam
        let currentJam = bs.jams.find((e) => { return e.period === bs.period && e.jamNumber === bs.jamNumber })
        let lineup = this.state.viewTeam === 'left'
            ? currentJam.leftRoster
            : currentJam.rightRoster

        return (<div>
            <h1>Lineups Tracker</h1>
            <ShortClockDisplay boutState={bs} />
            <ShortScoreDisplay boutState={bs} />

            <h2>Current Jam</h2>
            <Row>
                {lineup.map((e, i) => {
                    let name = team.roster.find((r) => { return e.number === r.number }).name
                    return (<Col sm={6} key={i}>
                        <Row className='lineups-skater'>
                            <Col sm={2} className='lineups-number'>
                                <Button onClick={() => {
                                    this.props.removeSkater(bs.boutId, this.state.viewPeriod, this.state.viewJam,
                                        this.state.viewTeam, e.number)
                                }}
                                    bsStyle='success' bsSize="large" block>{e.number}</Button>
                            </Col>
                            <Col sm={4} className='lineups-name'>{name}</Col>
                            <Col sm={2}>
                                <Button bsStyle={e.position === 0 ? 'success' : 'default'} onClick={() => {
                                    this.props.setSkaterPosition(bs.boutId, this.state.viewPeriod,
                                        this.state.viewJam, this.state.viewTeam, e.number, 0)
                                }} bsSize="large" block><Glyphicon glyph='bold' /></Button>
                            </Col>
                            <Col sm={2}>
                                <Button bsStyle={e.position === 1 ? 'success' : 'default'} onClick={() => {
                                    this.props.setSkaterPosition(bs.boutId, this.state.viewPeriod,
                                        this.state.viewJam, this.state.viewTeam, e.number, 1)
                                }} bsSize="large" block><Glyphicon glyph='star' /></Button>
                            </Col>
                            <Col sm={2}>
                                <Button bsStyle={e.position === 2 ? 'success' : 'default'} onClick={() => {
                                    this.props.setSkaterPosition(bs.boutId, this.state.viewPeriod,
                                        this.state.viewJam, this.state.viewTeam, e.number, 2)
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
                        //Red: In box but not jam
                        //Yellow: In box and jam?
                        //Green: In jam
                        //Blue: Sad, give her a hug

                        return (
                            <Col sm={6} key={i}>
                                <Row className='lineups-skater'>
                                    <Col sm={4} className='lineups-number'>
                                        <Button onClick={() => {
                                            this.props.addSkater(bs.boutId, this.state.viewPeriod, this.state.viewJam,
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
