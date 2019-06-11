import React from 'react';
import { Form } from 'react-bootstrap';

const Create = () => (
    <Form>
        <Form.Group controlId="form.Email">
            <Form.Control type="email" placeholder="Enter email" />
        </Form.Group>
        <Form.Group controlId="form.FirstName">
            <Form.Control type="text" placeholder="First Name" />
        </Form.Group>
        <Form.Group controlId="form.LastName">
            <Form.Control type="text" placeholder="Last Name" />
        </Form.Group>
    </Form>
);

export default Create;