import React from 'react';
import { Admin, Resource } from 'react-admin';
import DataProvider from './DataProvider';

import Dashboard from './Dashboard';

import { UserList } from './pages/UserList';
import { UserCreate } from './pages/UserCreate';
import { UserEdit } from './pages/UserEdit';
import { TeamList } from './pages/TeamList';

import UserIcon from '@material-ui/icons/Person';
import TeamIcon from '@material-ui/icons/Group';


const App = () => (
    <Admin dashboard={Dashboard} dataProvider={DataProvider}>
        <Resource name="users" list={UserList} edit={UserEdit} create={UserCreate} icon={UserIcon} />
        <Resource name="teams" list={TeamList} icon={TeamIcon} />
    </Admin>
);

export default App;