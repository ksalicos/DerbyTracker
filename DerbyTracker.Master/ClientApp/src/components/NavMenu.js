import React from 'react'
import { connect } from 'react-redux'
import { Glyphicon, Nav, Navbar, NavItem } from 'react-bootstrap'
import './NavMenu.css'
import { actionCreators as system } from '../store/System'

const NavMenu = props => (
    <Navbar fluid collapseOnSelect>
        <Navbar.Header>
            <Navbar.Brand>
                <span>Bettie</span>
            </Navbar.Brand>
            <Navbar.Toggle />
        </Navbar.Header>
        <Navbar.Collapse>
            <Nav>
                <NavItem onClick={() => { props.changeScreen('bout') }}>
                    <Glyphicon glyph='bullhorn' /> Bout Info
                </NavItem>
                <NavItem onClick={() => { props.changeScreen('nodes') }}>
                    <Glyphicon glyph='phone' /> Nodes
                </NavItem>
            </Nav>
        </Navbar.Collapse>
    </Navbar>
);

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
