import React from 'react'
const MatchupText = props => <span className='poppy'>
    {
        props.bout.leftTeam && props.bout.rightTeam
            ? <span>{props.bout.leftTeam.name} vs. {props.bout.rightTeam.name}</span>
            : 'Rosters Not Set'
    }</span>

export default MatchupText