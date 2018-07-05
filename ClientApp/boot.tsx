import './css/site.css';
import 'bootstrap';
import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { AppContainer } from 'react-hot-loader';
import RedditFeed from './components/RedditFeed';

//import { BrowserRouter } from 'react-router-dom';
//import * as RoutesModule from './routes';
//let routes = RoutesModule.routes;

function renderApp() {
    ReactDOM.render(
        <AppContainer>
            <div>
                <RedditFeed />
            </div>
        </AppContainer>,
        document.getElementById('react-app')
    );
}

renderApp();

// Allow Hot Module Replacement
if (module.hot) {
    //module.hot.accept('./routes', () => {
    //    routes = require<typeof RoutesModule>('./routes').routes;
    //    renderApp();
    //});
    module.hot.accept(() => {
        renderApp();
    });
}
