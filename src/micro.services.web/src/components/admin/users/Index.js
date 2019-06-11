import React , {Fragment} from 'react';
import List from './List';
import Toolbar from './Toolbar';

const UserAdmin = () => (
  <Fragment>
    <h1>User Administration</h1>
    <Toolbar></Toolbar>
    <br></br>
    <List></List>
  </Fragment>
);

export default UserAdmin;