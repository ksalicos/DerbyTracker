import React from 'react'
import { connect } from 'react-redux'
import { Button, Glyphicon, DropdownButton, MenuItem, Panel } from 'react-bootstrap'
import './Shared.css'
import { actionCreators as jam } from '../../store/CurrentJam'

class JamSelector extends React.Component {
    render() {
        let props = this.props
        //props 
        //  setJam
        //  currentIdx
        //  jams

        let bs = this.props.boutState.current
        let currentJam = bs.jams[this.props.jam.index]
        let lastIdx = bs.jams.length - 1

        let increment = () => props.setJam(props.jam.index + 1)
        let decrement = () => props.setJam(props.jam.index - 1)
        let first = () => props.setJam(0)
        let last = () => props.setJam(lastIdx)

        return (<Panel className='jam-selector'><Panel.Body>
            <Button onClick={first} disabled={props.jam.index === 0}><Glyphicon glyph="fast-backward" /></Button>
            <Button onClick={decrement} disabled={props.jam.index === 0}><Glyphicon glyph="arrow-left" /></Button>
            <span className='jam-label'>Viewing Period {currentJam.period} Jam {currentJam.jamNumber}</span>
            <Button onClick={increment} disabled={props.jam.index === lastIdx}><Glyphicon glyph="arrow-right" /></Button>
            <Button onClick={last} disabled={props.jam.index === lastIdx}><Glyphicon glyph="fast-forward" /></Button>
            <div className='jam-jump'>
                <DropdownButton title='Jump to Jam' pullRight={true} id='jam-jump-id'>
                    {
                        bs.jams.map((e, i) => <MenuItem onClick={() => { props.setJam(i) }} key={i}>
                            Period:{e.period}  Jam: {e.jamNumber}
                        </MenuItem>)
                    }
                </DropdownButton>
            </div>
            <div className='done-button'>
                {
                    props.jam.ready
                        ? <Button bsStyle='primary' onClick={() => props.setready(false)}>Done!</Button>
                        : <Button onClick={() => props.setready(true)}>Done!</Button>

                }
            </div>
        </Panel.Body></Panel>)
    }
}

const mapStateToProps = state => {
    return {
        system: state.system,
        boutState: state.boutState,
        jam: state.currentJam
    }
}

const mapDispatchToProps = dispatch => {
    return {
        setJam: (n) => dispatch(jam.setJam(n)),
        setready: (r) => dispatch(jam.setReady(r)),
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(JamSelector);