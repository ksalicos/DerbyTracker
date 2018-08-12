import React from 'react'
import { connect } from 'react-redux'
import { Glyphicon, Nav, Navbar, MenuItem, NavDropdown } from 'react-bootstrap'
import './NavMenu.css'
import { actionCreators as system } from '../store/System'
import screenfull from 'screenfull'

const roles = [
    { role: 'JamTimer', label: 'Jam Timer', glyph: 'time' },
    { role: 'LineupsTracker', label: 'Lineups Tracker', glyph: 'user' },
    { role: 'ScoreKeeper', label: 'Score Keeper', glyph: 'blackboard' },
    { role: 'PenaltyTracker', label: 'Penalty Tracker', glyph: 'eye-open' },
    { role: 'PenaltyBoxTimer', label: 'Penalty Box', glyph: 'inbox' },
    { role: 'HeadNSO', label: 'Head NSO', glyph: 'sunglasses' },
]

//const unsecured = [
//add screens that dont need permission, ie stats and scoreboard
//]

const NavMenu = props => {
    let inRoles = roles.filter((e) => { return props.system.roles.includes(e.role) })
    let title = props.system.screen === 'bout' ? 'Awaiting Role' : props.system.screen
    return (
        <Navbar collapseOnSelect>
            <Navbar.Header>
                <Navbar.Brand>
                    <span>Node# {props.system.connectionNumber}</span>
                </Navbar.Brand>
                <Navbar.Toggle />
            </Navbar.Header>
            <Navbar.Collapse>
                <Nav>
                    <NavDropdown eventKey={3} title={title} id="nav-dropdown">
                        {
                            inRoles.map((e, i) =>
                                <MenuItem key={i} eventKey={i} onClick={() => { props.changeScreen(e.role) }}>
                                    <Glyphicon glyph={e.glyph} /> {e.label}
                                </MenuItem>
                            )
                        }
                        {
                            screenfull.enabled
                                ? <MenuItem onClick={() => screenfull.toggle()}>
                                    <Glyphicon glyph='resize-small' /> Toggle Full Screen </MenuItem>
                                : null
                        }
                    </NavDropdown>
                </Nav>
            </Navbar.Collapse>
        </Navbar>
    )
};

const mapStateToProps = state => {
    return {
        system: state.system
    }
}

const mapDispatchToProps = dispatch => {
    return {
        changeScreen: (s) => dispatch(system.changeScreen(s))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(NavMenu);
