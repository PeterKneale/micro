import React from 'react';
import { Create, SimpleForm, TextInput } from 'react-admin';

export const UserCreate = props => (
    <Create {...props}>
        <SimpleForm>
            <TextInput source="firstname" />
            <TextInput source="lastname" />
            <TextInput source="email" />
        </SimpleForm>
    </Create>
);