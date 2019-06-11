import React from 'react';
import { Table, Button } from 'react-bootstrap';

const List = () => (
    <Table>
      <thead>
        <tr>
          <th></th>
          <th>First Name</th>
          <th>Last Name</th>
          <th>Email</th>
          <th colspan="2"></th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td><Button variant="link" size="sm" href="users/view/1">View</Button></td>
          <td>Peter</td>
          <td>Kneale</td>
          <td>peterkneale@gmail.com</td>
          <td><Button variant="primary" size="sm">Edit</Button></td>
          <td><Button variant="primary" size="sm">Delete</Button></td>
        </tr>
        <tr>
          <td><Button variant="link" size="sm" href="users/view/2">View</Button></td>
          <td>Peter</td>
          <td>Kneale</td>
          <td>peterkneale@gmail.com</td>
          <td><Button variant="primary" size="sm">Edit</Button></td>
          <td><Button variant="primary" size="sm">Delete</Button></td>
        </tr>
      </tbody>
    </Table>
);

export default List;