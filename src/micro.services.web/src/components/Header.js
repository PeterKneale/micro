import React from 'react';
import { Nav, Navbar } from 'react-bootstrap';

const Header = () => (
    <Navbar bg="light">
        <Navbar.Brand href="/">Micro</Navbar.Brand>
        <Nav.Link href="/admin/users">Users</Nav.Link>
    </Navbar>
);

export default Header;