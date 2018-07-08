import React from 'react'
import { connect } from 'react-redux'
import { actionCreators as bout } from '../../store/Bout'
import agent from 'superagent'
import Moment from 'react-moment';

class BoutList extends React.Component {
    render() {
        return (<div>
            <h1>Bout List</h1>
            <table>
                <tbody>
                    <tr><th>Name</th><th>Last Edited</th></tr>
                    {this.props.bout.list.map(b => <tr className={'bout'} key={b.id}
                        onClick={() => { this.props.load(b.id) }}>
                        <td>{b.name}</td>
                        <td><Moment format="MMM DD YYYY, h:mmA">{b.timeStamp}</Moment></td>
                    </tr>)}
                </tbody>
            </table>
            <button onClick={this.props.create}>Create</button>
        </div>)
    }
}

const mapStateToProps = state => {
    return {
        bout: state.bout
    }
}

const mapDispatchToProps = dispatch => {
    return {
        create: () =>
            dispatch(bout.create()),
        load: (id) => {
            agent.get('/api/bout/load/' + id)
                .then((r) => {
                    dispatch(bout.boutLoaded(r.body))
                })
        }
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(BoutList);