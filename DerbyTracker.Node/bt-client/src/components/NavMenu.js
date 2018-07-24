import React from 'react'
import { connect } from 'react-redux'
import { Glyphicon, Nav, Navbar, NavItem } from 'react-bootstrap'
import './NavMenu.css'
import { actionCreators as system } from '../store/System'

const roles = [
    { role: 'JamTimer', label: 'Jam Timer', glyph: 'time' },
    { role: 'LineupsTracker', label: 'Lineups Tracker', glyph: 'user' },
    { role: 'ScoreKeeper', label: 'Score Keeper', glyph: 'blackboard' },
    { role: 'PenaltyTracker', label: 'Penalty Tracker', glyph: 'eye-open' },
    { role: 'Penalty Box', label: 'Penalty Box', glyph: 'inbox' },
    { role: 'HeadNSO', label: 'Head NSO', glyph: 'sunglasses' },
]

//const unsecured = [
//add screens that dont need permission, ie stats and scoreboard
//]

const NavMenu = props => {
    let inRoles = roles.filter((e) => { return props.system.roles.includes(e.role) })
    return (
        <Navbar fluid collapseOnSelect>
            <Navbar.Header>
                <Navbar.Brand>
                    <span>Node# {props.system.connectionNumber}</span>
                </Navbar.Brand>
                <Navbar.Toggle />
            </Navbar.Header>
            <Navbar.Collapse>
                {
                    inRoles.map((e, i) =>
                        <Nav key={i}>
                            <NavItem onClick={() => { props.changeScreen(e.role) }}>
                                <Glyphicon glyph={e.glyph} /> {e.label}
                            </NavItem>
                        </Nav>
                    )
                }
                <Nav>
                    <NavItem onClick={() => { props.changeScreen('scoreboard') }}>
                        <Glyphicon glyph='film' /> Scoreboard
                    </NavItem>
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
