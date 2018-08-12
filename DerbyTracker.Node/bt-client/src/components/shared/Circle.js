import React from 'react'

const Circle = props => <svg viewBox="0 0 100 100" className={props.className}>
    <circle cx="50" cy="50" r="40" stroke="black" strokeWidth="3" fill={props.color} />
</svg>

export default Circle