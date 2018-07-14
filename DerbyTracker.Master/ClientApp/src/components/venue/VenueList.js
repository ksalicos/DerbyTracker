import React from 'react'
import { connect } from 'react-redux'
import { Table } from 'react-bootstrap'
import { actionCreators as venue } from '../../store/Venue'
import { actionCreators as system } from '../../store/System'
import { actionCreators as bout } from '../../store/Bout'
import uuid from 'uuid'

const VenueList = props =>
    <div>
        <Table>
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Location</th>
                    <th></th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                {props.venue.list.map((e, i) => <tr key={i}>
                    <td>
                        {e.name}
                    </td>
                    <td>
                        {e.city}, {e.state}
                    </td>
                    <td>
                        <button onClick={() => { props.selectVenue(e) }}>Select</button>
                    </td>
                    <td>
                        <button onClick={() => { props.editVenue(e) }}>Edit</button>
                    </td>

                </tr>)}
            </tbody>
        </Table>


        <button onClick={props.newVenue}>New Venue</button>
        <button onClick={props.returnToBout}>Cancel</button>
    </div>

const mapStateToProps = state => {
    return {
        venue: state.venue
    }
}

const mapDispatchToProps = dispatch => {
    return {
        newVenue: () => { dispatch(venue.editVenue({ ...initialVenue, id: uuid.v4() })) },
        editVenue: (b) => { dispatch(venue.editVenue(b)) },
        returnToBout: () => { dispatch(system.changeScreen('bout')) },
        selectVenue: (b) => { dispatch(bout.venueSelected(b)); }
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(VenueList);

const initialVenue = {
    name: 'New Venue',
    city: 'Portland',
    state: 'OR'
}