import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import RedditFeed from './RedditFeed';

export class Home extends React.Component<RouteComponentProps<{}>, {}> {
    public render() {
        return (
            <div>
                <RedditFeed />
            </div>
        )
    }
}
