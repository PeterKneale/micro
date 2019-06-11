import React from 'react';
import { Edit, SimpleForm, DisabledInput, TextInput } from 'react-admin';

const UserTitle = ({ record }) => {
    return <span>User {record ? `"${record.name}"` : ''}</span>;
};

export const UserEdit = props => (
    <Edit title={<UserTitle />} {...props}>
        <SimpleForm>
            <DisabledInput source="id" />
            <TextInput source="firstname" />
            <TextInput source="lastname" />
            <TextInput source="email" />
        </SimpleForm>
    </Edit>
);