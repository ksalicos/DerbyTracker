import React from 'react'
import { connect } from 'react-redux'
import VenueEdit from './venue/VenueEdit'
import VenueList from './venue/VenueList'

const Venue = props =>
    <div>
        <h1>Venue</h1>
        {props.venue.current ? <VenueEdit /> : <VenueList />}
    </div>

const mapStateToProps = state => {
    return { venue: state.venue }
}

export default connect(mapStateToProps)(Venue);
