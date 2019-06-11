import React from 'react';
import { List, Datagrid, TextField, EmailField, ReferenceField, EditButton } from 'react-admin';

export const UserList = props => (
    <List {...props}>
        <Datagrid>
            <ReferenceField source="id" reference="users">
                <TextField source="name" />
            </ReferenceField>
            <EmailField source="email" />
            <EditButton />
        </Datagrid>
    </List>
);