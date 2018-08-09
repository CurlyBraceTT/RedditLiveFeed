import * as React from 'react';
import { Route } from 'react-router-dom';
import { Home, Layout } from './scenes';

export const routes = <Layout>
    <Route exact path='/' component={Home} />
    <Route exact path='/r/:feedId' component={Home} />
</Layout>;