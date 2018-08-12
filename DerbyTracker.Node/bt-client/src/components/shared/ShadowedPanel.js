import React from 'react'
import { Panel } from 'react-bootstrap'

const ShadowedPanel = props => {
    let shadow = props.subtle
        ? `2px 2px 2px ${props.color}`
        : `3px 0 3px ${props.color}, 0 3px 3px ${props.color}, 3px 3px 3px ${props.color}, -1px -1px 3px ${props.color}`
    return <div className='panel panel-default'
        style={{
            boxShadow: shadow
        }}>
        <Panel.Body>
            {props.children}
        </Panel.Body>
    </div >
}

export default ShadowedPanel