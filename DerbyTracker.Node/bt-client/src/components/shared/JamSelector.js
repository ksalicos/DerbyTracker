import React from 'react'
import { Button, Glyphicon, DropdownButton, MenuItem } from 'react-bootstrap'
import './Shared.css'

const JamSelector = props => {
    //props 
    //  setJam
    //  currentIdx
    //  jams

    let lastIdx = props.jams.length - 1
    let currentJam = props.jams[props.currentIdx]

    let increment = () => props.setJam(props.currentIdx + 1)
    let decrement = () => props.setJam(props.currentIdx - 1)
    let first = () => props.setJam(0)
    let last = () => props.setJam(lastIdx)

    return (<div className='jam-selector'>
        <Button onClick={first} disabled={props.currentIdx === 0}><Glyphicon glyph="fast-backward" /></Button>
        <Button onClick={decrement} disabled={props.currentIdx === 0}><Glyphicon glyph="arrow-left" /></Button>
        <span>Viewing Period {currentJam.period} Jam {currentJam.jamNumber}</span>
        <Button onClick={increment} disabled={props.currentIdx === lastIdx}><Glyphicon glyph="arrow-right" /></Button>
        <Button onClick={last} disabled={props.currentIdx === lastIdx}><Glyphicon glyph="fast-forward" /></Button>
        <div className='jam-jump'>
            <DropdownButton title='Jump to Jam' pullRight={true} id='jam-jump-id'>
                {
                    props.jams.map((e, i) => <MenuItem onClick={() => { props.setJam(i) }} key={i}>
                        Period:{e.period}  Jam: {e.jamNumber}
                    </MenuItem>)
                }
            </DropdownButton>
        </div>
    </div>)
}

export default JamSelector