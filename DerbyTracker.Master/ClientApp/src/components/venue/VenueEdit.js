import React from 'react'
import { connect } from 'react-redux'
import { actionCreators as venue } from '../../store/Venue'
import LabelledInput from '../common/LabelledTextInput'
import agent from 'superagent'

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
                <LabelledInput config={{
                    name: 'name', label: 'Name', value: this.state.name,
                    onChange: this.handleChange, disabled: this.state.busy
                }} />
                <LabelledInput config={{
                    name: 'city', label: 'City', value: this.state.city,
                    onChange: this.handleChange, disabled: this.state.busy
                }} />
                <LabelledInput config={{
                    name: 'state', label: 'State', value: this.state.state,
                    onChange: this.handleChange, disabled: this.state.busy
                }} />

                <button disabled={this.state.saved} onClick={this.save}>Save</button>
                <button onClick={this.props.exit}>Exit</button>
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
            Id: this.state.id,
            name: this.state.name,
            city: this.state.city,
            state: this.state.state
        }
        agent.post('/api/venue/save')
            .send(venue)
            .set('Accept', 'application/json')
            .then((r) => {
                this.setState({ busy: false, saved: true })
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
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(VenueEdit);