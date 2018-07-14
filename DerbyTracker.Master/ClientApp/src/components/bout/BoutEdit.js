import React from 'react'
import { connect } from 'react-redux'
import { actionCreators as bout } from '../../store/Bout'
import { actionCreators as system } from '../../store/System'
import LabelledInput from '../common/LabelledTextInput'
import VenueShortDetails from '../venue/VenueShortDetails';

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

        this.handleChange = this.handleChange.bind(this);
        this.exit = this.exit.bind(this);
    }

    render() {
        return (
            <div>
                <h1>Bout Edit</h1>
                <LabelledInput config={{
                    name: 'name', label: 'Name', value: this.state.name,
                    onChange: this.handleChange, disabled: this.state.busy
                }} />
                <VenueShortDetails venue={this.props.bout.current.venue} />
                <button onClick={this.props.toVenueDetails}>Edit Venue</button>
                <LabelledInput config={{
                    name: 'advertisedStart', label: 'Date/Time', value: this.state.advertisedStart,
                    onChange: this.handleChange, disabled: this.state.busy
                }} />

                <button onClick={this.props.save}>Save</button>
                <button onClick={this.exit}>Exit</button>
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
        }, () => {
            let bout = {
                boutId: this.state.boutId,
                name: this.state.name,
                advertisedStart: this.state.advertisedStart
            }
            this.props.updateCurrent(bout)
        });
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
        save: () => dispatch(bout.saveBout())
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(BoutEdit);