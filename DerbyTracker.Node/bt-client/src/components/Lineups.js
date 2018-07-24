
import React from 'react'
import { connect } from 'react-redux'
import { actionCreators as system } from '../store/System'
import { actionCreators as lineupsTracker } from '../store/lineupsTrackerSignalR'
import ShortClockDisplay from './shared/ShortClockDisplay';
import ShortScoreDisplay from './shared/ShortScoreDisplay';
import { Row, Col, Button, ButtonGroup } from 'react-bootstrap'

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

        return (<div>
            <h1>Lineups Tracker</h1>
            <ShortClockDisplay boutState={bs} />
            <ShortScoreDisplay boutState={bs} />

            <h2>Current Jam Here</h2>

            <h2>
                <ButtonGroup bsSize="large">
                    <Button bsStyle={this.state.viewTeam === 'left' ? 'primary' : ''}
                        onClick={() => { this.setState({ viewTeam: 'left' }) }}>
                        {data.leftTeam.name}
                    </Button>
                    <Button bsStyle={this.state.viewTeam === 'right' ? 'primary' : ''}
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
                                        <Button bsStyle={buttonStyle} bsSize="large" block>{e.number}</Button>
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
        go: () => dispatch(system.changeScreen('bout')),
        addSkater: (period, jam, number) => dispatch(lineupsTracker.addSkater(period, jam, number))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(LineupsTracker);
