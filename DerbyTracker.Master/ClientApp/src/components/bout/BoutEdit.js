import React from 'react'
import { connect } from 'react-redux'
import { actionCreators as bout } from '../../store/Bout'
import LabelledInput from '../common/LabelledTextInput'
import agent from 'superagent'

class BoutEdit extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            busy: false,
            saved: true,
            boutId: props.bout.current.boutId,
            name: props.bout.current.name,
            advertisedStart: props.bout.current.advertisedStart,
            venue: props.bout.current.venue
        }

        this.handleChange = this.handleChange.bind(this);
        this.save = this.save.bind(this);
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
                <LabelledInput config={{
                    name: 'venue', label: 'Venue', value: this.state.venue,
                    onChange: this.handleChange, disabled: this.state.busy
                }} />
                <LabelledInput config={{
                    name: 'advertisedStart', label: 'Date/Time', value: this.state.advertisedStart,
                    onChange: this.handleChange, disabled: this.state.busy
                }} />

                <button disabled={this.state.saved} onClick={this.save}>Save</button>
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
        });
    }

    save() {
        let bout = {
            boutId: this.state.boutId,
            name: this.state.name,
            advertisedStart: this.state.advertisedStart,
            venue: this.state.venue
        }
        agent.post('/api/bout/save')
            .send(bout)
            .set('Accept', 'application/json')
            .then((r) => {
                this.setState({ busy: false, saved: true, boutId: r.body })
                this.props.updateCurrent(bout)
            })
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
        updateCurrent: (b) => dispatch(bout.boutUpdated(b))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(BoutEdit);