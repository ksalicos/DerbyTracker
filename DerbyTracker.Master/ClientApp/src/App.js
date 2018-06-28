import React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import LoadingScreen from './components/LoadingScreen'

export default () => (
    <Layout>
        <Route exact path='/' component={LoadingScreen} />
        <Route exact path='/home' component={Home} />
    </Layout>
);
