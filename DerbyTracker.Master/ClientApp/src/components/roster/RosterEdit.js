import React from 'react'
import { connect } from 'react-redux'
import { actionCreators as bout } from '../../store/Bout'
import { actionCreators as roster } from '../../store/Roster'
import { Row, Col } from 'react-bootstrap'

class RosterEdit extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            busy: false,
            name: this.props.roster.current.name,
            color: this.props.roster.current.color || '#ff00ff',
            roster: this.props.roster.current.roster,
            newname: '',
            newnumber: ''
        }

        this.handleChange = this.handleChange.bind(this);
        this.save = this.save.bind(this);
        this.add = this.add.bind(this);
    }

    render() {
        return (
            <div>
                <h1>Team Edit</h1>
                <Row>
                    <Col sm={6}>
                        <Row>
                            <Col sm={1}><label className='poppy'>Name</label></Col>
                            <Col sm={5}>
                                <input name='name' value={this.state.name} onChange={this.handleChange} disabled={this.state.busy} />
                            </Col>
                        </Row>

                        <Row>
                            <Col sm={1}><label className='poppy'>Color</label></Col>
                            <Col sm={5}>
                                <input name='color' value={this.state.color} onChange={this.handleChange} disabled={this.state.busy} type='color' />
                            </Col>
                        </Row>

                        {
                            this.state.roster.map((e, i) =>
                                <Row key={i}>
                                    <Col sm={4}>
                                        <input value={e.number} name={'number|' + i} onChange={this.handleChange} />
                                    </Col>
                                    <Col sm={4}>
                                        <input value={e.name} name={'name|' + i} onChange={this.handleChange} />
                                    </Col>
                                </Row>
                            )
                        }

                        <Row>
                            <Col sm={4}>
                                <input value={this.state.newnumber} onChange={this.handleChange} name='newnumber' />
                            </Col>
                            <Col sm={4}>
                                <input value={this.state.newname} onChange={this.handleChange} name='newname' />
                            </Col>
                            <Col sm={4}>
                                <button onClick={this.add}>Add</button>
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
        let idx
        if (target.name.startsWith('number')) {
            idx = target.name.split('|')[1]
            this.setState({ roster: this.state.roster.map((e, i) => i === idx * 1 ? { ...e, number: target.value } : e), saved: false })
        } else if (target.name.startsWith('name')) {
            idx = target.name.split('|')[1]
            this.setState({ roster: this.state.roster.map((e, i) => i === idx * 1 ? { ...e, name: target.value } : e), saved: false })
        } else {
            this.setState({ [target.name]: target.value, saved: false });
        }
    }

    save() {
        let team = {
            name: this.state.name,
            roster: this.state.roster,
            color: this.state.color,
        }
        this.props.save(team, this.props.roster.selectedSide)
    }

    add() {
        let newRoster = [...this.state.roster]
        newRoster.push({ name: this.state.newname, number: this.state.newnumber })
        this.setState({ roster: newRoster, newname: '', newnumber: '' })
    }
}

const mapStateToProps = state => {
    return {
        roster: state.roster
    }
}

const mapDispatchToProps = dispatch => {
    return {
        exit: () => dispatch(roster.rosterSelected(null, null)),
        save: (r, s) => dispatch(bout.rosterUpdated(r, s))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(RosterEdit)