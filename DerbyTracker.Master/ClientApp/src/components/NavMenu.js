import React from 'react'
import { Glyphicon, Nav, Navbar, NavItem } from 'react-bootstrap'
import './NavMenu.css'

const foo = props => (
    <Navbar fluid collapseOnSelect>
        <Navbar.Header>
            <Navbar.Brand>
                <span>Bettie</span>
            </Navbar.Brand>
            <Navbar.Toggle />
        </Navbar.Header>
        <Navbar.Collapse>
            <Nav>
                <NavItem>
                    <Glyphicon glyph='bullhorn' /> Bout Info
                </NavItem>
                <NavItem>
                    <Glyphicon glyph='phone' /> Nodes
                </NavItem>
            </Nav>
        </Navbar.Collapse>
    </Navbar>
);

export default foo