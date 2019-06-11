import React from 'react';
import { ButtonToolbar, Button } from 'react-bootstrap';

const Toolbar = () => (
  <ButtonToolbar>
    <Button variant="primary" href="users/create" >Create</Button>
  </ButtonToolbar>
);

export default Toolbar;