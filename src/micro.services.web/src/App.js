import React from 'react';
import { BrowserRouter as Router, Route } from "react-router-dom";
import { Container } from 'react-bootstrap';
import Header from './components/Header';
import Home from './components/Home';
import UserAdmin from './components/admin/users/Index';
import Create from './components/admin/users/Create';
import View from './components/admin/users/View';

function App() {
  return (
    <Router>
      <Header/>
      <Container>
      <Route path="/" exact component={Home}></Route>
      <Route path="/admin/users" exact component={UserAdmin}></Route>
      <Route path="/admin/users/create" exact component={Create}></Route>
      <Route path="/admin/users/view/:id" exact component={View}></Route>
      </Container>
    </Router>
  );
}

export default App;
