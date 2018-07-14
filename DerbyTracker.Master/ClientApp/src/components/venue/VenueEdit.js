import React from 'react'
import { connect } from 'react-redux'
import { actionCreators as venue } from '../../store/Venue'
import agent from 'superagent'
import { Row, Col } from 'react-bootstrap'

class VenueEdit extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            busy: false,
            saved: true,
            id: props.venue.current.id,
            name: props.venue.current.name,
            city: props.venue.current.city,
            state: props.venue.current.state,
        }

        this.handleChange = this.handleChange.bind(this);
        this.save = this.save.bind(this);
    }

    render() {
        return (
            <div>
                <h1>Venue Edit</h1>
                <Row>
                    <Col sm={6}>
                        <Row>
                            <Col sm={1}><label>Name</label></Col>
                            <Col sm={5}>
                                <input name='name' value={this.state.name} onChange={this.handleChange} disabled={this.state.busy} />
                            </Col>
                        </Row>
                        <Row>
                            <Col sm={1}><label>Sity</label></Col>
                            <Col sm={5}>
                                <input name='city' value={this.state.city} onChange={this.handleChange} disabled={this.state.busy} />
                            </Col>
                        </Row>
                        <Row>
                            <Col sm={1}><label>State</label></Col>
                            <Col sm={5}>
                                <input name='state' value={this.state.state} onChange={this.handleChange} disabled={this.state.busy} />
                            </Col>
                        </Row>
                    </Col>
                </Row>
                <div>
                    <button disabled={this.state.busy} onClick={this.save}>Save</button>
                    <button disabled={this.state.busy} onClick={this.props.exit}>Cancel</button>
                </div>
            </div>
        )
    }

    handleChange(event) {
        const target = event.target;
        const value = target.value;
        const name = target.name;

        this.setState({
            [name]: value,
            saved: false
        });
    }

    save() {
        let venue = {
            id: this.state.id,
            name: this.state.name,
            city: this.state.city,
            state: this.state.state
        }
        agent.post('/api/venue/save')
            .send(venue)
            .set('Accept', 'application/json')
            .then((r) => {
                this.props.save(venue)
            })
    }
}

const mapStateToProps = state => {
    return {
        venue: state.venue
    }
}

const mapDispatchToProps = dispatch => {
    return {
        exit: () => dispatch(venue.editVenue(null)),
        save: (v) => dispatch(venue.venueUpdated(v))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(VenueEdit)