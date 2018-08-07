import React from 'react'
const MatchupText = props => <span className='poppy'>
    {
        props.bout.left && props.bout.right
            ? <span>{props.bout.left.name} vs. {props.bout.right.name}</span>
            : 'Rosters Not Set'
    }</span>

export default MatchupText