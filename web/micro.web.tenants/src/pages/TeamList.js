import React from 'react';
import { List, Datagrid, TextField, ReferenceField } from 'react-admin';

export const TeamList = props => (
    <List {...props}>
        <Datagrid rowClick="edit">
            <ReferenceField source="id" reference="teams">
                <TextField source="name" />
            </ReferenceField>
        </Datagrid>
    </List>
);