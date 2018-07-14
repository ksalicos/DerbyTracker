import React from 'react'
import { connect } from 'react-redux'
import { actionCreators as venue } from '../../store/Venue'
import { actionCreators as system } from '../../store/System'
import { actionCreators as bout } from '../../store/Bout'
import uuid from 'uuid'

const VenueList = props =>
    <div>
        {props.venue.list.map((e, i) => <div key={i}>
            {e.name} {e.city},{e.state}
            <button onClick={() => { props.selectVenue(e) }}>Select</button>
            <button onClick={() => { props.editVenue(e) }}>Edit</button>
        </div>)}
        <button onClick={props.newVenue}>New Venue</button>
        <button onClick={props.returnToBout}>Return</button>
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
        selectVenue: (b) => { dispatch(bout.selectVenue(b)) }
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(VenueList);

const initialVenue = {
    name: 'New Venue',
    city: 'Portland',
    state: 'OR'
}