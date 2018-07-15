import React from 'react'
import { connect } from 'react-redux'
import { actionCreators as bout } from '../../store/Bout'
import { actionCreators as system } from '../../store/System'
import DateTime from 'react-datetime'
import 'react-datetime/css/react-datetime.css'
import { Col, Row } from 'react-bootstrap'
import MatchupText from './MatchupText'

class BoutEdit extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            busy: false,
            saved: true,
            boutId: props.bout.current.boutId,
            name: props.bout.current.name,
            advertisedStart: props.bout.current.advertisedStart
        }

        this.handleChange = this.handleChange.bind(this)
        this.timeChange = this.timeChange.bind(this)
        this.updateBout = this.updateBout.bind(this)
        this.exit = this.exit.bind(this)
    }

    render() {
        let venue = this.props.bout.current.venue

        return (
            <div>
                <h1>Edit Bout</h1>
                <Row>
                    <Col sm={4}>
                        <Row>
                            <Col sm={2}><label>Name</label></Col>
                            <Col sm={10}>
                                <input name='name' value={this.state.name} onChange={this.handleChange} />
                            </Col>
                        </Row>

                    </Col>
                    <Col sm={6}>
                        <div className='poppy'>Venue:
                                {venue ? <span>{venue.name} {venue.city}, {venue.state}</span>
                                : <span>No Venue Set</span>}
                        </div>
                        <button onClick={this.props.toVenueDetails}>Edit Venue</button>
                    </Col>
                </Row>

                <Row>
                    <Col sm={4}>
                        <p>Date/Time</p>
                        <DateTime onChange={this.timeChange} input={true} defaultValue={this.state.advertisedStart} />
                    </Col>
                    <Col sm={4}>
                        <div><MatchupText bout={this.props.bout.current} /></div>
                        <button onClick={this.props.editRosters}>Edit Rosters</button>
                    </Col>
                </Row>

                <Row>
                    <button onClick={this.props.save}>Save</button>
                    <button onClick={this.exit}>Cancel</button>
                </Row>

            </div>
        )
    }

    timeChange(timestamp) {
        this.setState({ advertisedStart: timestamp.format('MM/DD/YYYY hh:mm a') }, this.updateBout)
    }

    handleChange(event) {
        const target = event.target;
        const value = target.value;
        const name = target.name;

        this.setState({
            [name]: value,
            saved: false
        }, this.updateBout);
    }

    updateBout() {
        let bout = {
            boutId: this.state.boutId,
            name: this.state.name,
            advertisedStart: this.state.advertisedStart
        }
        this.props.updateCurrent(bout)
    }

    exit() {
        if (this.state.boutId === '00000000-0000-0000-0000-000000000000') {
            this.props.exit()
        } else {
            this.props.toggleEdit()
        }
    }
}

const mapStateToProps = state => {
    return {
        bout: state.bout
    }
}

const mapDispatchToProps = dispatch => {
    return {
        exit: () => dispatch(bout.exit()),
        toggleEdit: () => dispatch(bout.toggleEdit()),
        updateCurrent: (b) => dispatch(bout.boutUpdated(b)),
        toVenueDetails: () => dispatch(system.changeScreen('venue')),
        save: () => dispatch(bout.saveBout()),
        editRosters: () => dispatch(system.changeScreen('rosters'))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(BoutEdit);