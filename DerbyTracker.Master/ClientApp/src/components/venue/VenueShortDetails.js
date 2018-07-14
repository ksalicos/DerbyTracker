import React from 'react'

const VenueShortDetails = props => <div>{
    props.venue ?
        <p>{props.venue.name} {props.venue.city}, {props.venue.state}</p>
        : <p>No Venue Set</p>
}</div>

export default VenueShortDetails