
import React from 'react'
import { connect } from 'react-redux'
import { actionCreators as lineupsTracker } from '../store/lineupsTrackerSignalR'
import ShortClockDisplay from './shared/ShortClockDisplay';
import ShortScoreDisplay from './shared/ShortScoreDisplay';
import { Row, Col, Button, ButtonGroup, Glyphicon } from 'react-bootstrap'
import JamSelector from './shared/JamSelector'
import './Lineups.css'

class LineupsTracker extends React.Component {
    constructor(props) {
        super(props)

        this.state = {
            viewTeam: 'left',
            viewPeriod: 1,
            viewJam: 1,
            jamIndex: props.boutState ? props.boutState.current.jams.length - 1 : null
        }
    }

    render() {
        let sort = (a, b) => a.number + '' < b.number + '' ? -1 : 1
        let bs = this.props.boutState.current
        let data = this.props.boutState.data
        let team = data[this.state.viewTeam]

        team.roster.sort(sort)
        let currentJam = bs.jams[this.state.jamIndex]
        let lineup = currentJam[this.state.viewTeam].roster
            .sort(sort)

        let borderColor = this.state.viewTeam === 'left' ? this.props.boutState.data.left.color
            : this.props.boutState.data.right.color

        let skaterLine = (e, i) => {
            let inLineup = lineup.some(l => l.number === e.number)
            let isJammer = lineup.some(l => l.number === e.number && l.position === 1)
            let isPivot = lineup.some(l => l.number === e.number && l.position === 2)
            let isBlocker = inLineup && !(isJammer || isPivot)

            let buttonStyle = 'default'
            if (inLineup) {
                buttonStyle = 'primary'
            }

            return (
                <Row key={i} className='lineups-skater'>
                    <Col sm={2} >
                        <Button className='lineups-number' bsStyle={buttonStyle} bsSize="large" block onClick={() => {
                            inLineup
                                ? this.props.removeSkater(bs.boutId, currentJam.period, currentJam.jamNumber,
                                    this.state.viewTeam, e.number)
                                : this.props.addSkater(bs.boutId, currentJam.period, currentJam.jamNumber,
                                    this.state.viewTeam, e.number)
                        }}>{e.number}</Button>
                    </Col>
                    <Col sm={5} lg={6} className='lineups-name'>{e.name}</Col>
                    <Col sm={5} lg={4}>
                        <ButtonGroup bsSize="large">
                            <Button bsStyle={isBlocker ? 'primary' : 'default'} onClick={() => {
                                this.props.setSkaterPosition(bs.boutId, currentJam.period, currentJam.jamNumber,
                                    this.state.viewTeam, e.number, 0)
                            }} disabled={!inLineup} ><Glyphicon glyph='bold' /></Button>
                            <Button bsStyle={isJammer ? 'primary' : 'default'} onClick={() => {
                                this.props.setSkaterPosition(bs.boutId, currentJam.period, currentJam.jamNumber,
                                    this.state.viewTeam, e.number, 1)
                            }} disabled={!inLineup} ><Glyphicon glyph='star' /></Button>
                            <Button bsStyle={isPivot ? 'primary' : 'default'} onClick={() => {
                                this.props.setSkaterPosition(bs.boutId, currentJam.period, currentJam.jamNumber,
                                    this.state.viewTeam, e.number, 2)
                            }} disabled={!inLineup} ><Glyphicon glyph='stop' /></Button>
                        </ButtonGroup>
                    </Col>
                </Row>
            )
        }

        return (<div>
            <ShortClockDisplay boutState={bs} />
            <ShortScoreDisplay boutState={bs} />

            <JamSelector setJam={(j) => this.setState({ jamIndex: j })} currentIdx={this.state.jamIndex}
                jams={bs.jams} />

            <div className='team-select'>
                <ButtonGroup bsSize="large">
                    <Button bsStyle={this.state.viewTeam === 'left' ? 'primary' : 'default'}
                        onClick={() => { this.setState({ viewTeam: 'left' }) }}>
                        {data['left'].name}
                    </Button>
                    <Button bsStyle={this.state.viewTeam === 'right' ? 'primary' : 'default'}
                        onClick={() => { this.setState({ viewTeam: 'right' }) }}>
                        {data['right'].name}
                    </Button>
                </ButtonGroup>
            </div>

            <Row className='lineups-roster' style={{ 'borderColor': borderColor }} >
                <Col sm={12} md={6} >
                    {
                        team.roster.filter((e, i) => i < (team.roster.length + 1) / 2).map(skaterLine)
                    }
                </Col>
                <Col sm={12} md={6}  >
                    {
                        team.roster.filter((e, i) => i >= (team.roster.length + 1) / 2).map(skaterLine)
                    }
                </Col>
            </Row>

            <Row>
                <Col sm={6}>
                    {lineup.filter((e, i) => i < (lineup.length + lineup.length % 2) / 2).map((e, i) => {
                        let s = { ...e, name: team.roster.find((r) => { return e.number === r.number }).name }
                        return skaterLine(s, i)
                    })}
                </Col>
                <Col sm={6}>
                    {lineup.filter((e, i) => i >= (lineup.length + lineup.length % 2) / 2).map((e, i) => {
                        let s = { ...e, name: team.roster.find((r) => { return e.number === r.number }).name }
                        return skaterLine(s, i)
                    })}
                </Col>
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
