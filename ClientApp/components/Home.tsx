import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import RedditFeed from './RedditFeed';

export class Home extends React.Component<RouteComponentProps<{}>, {}> {
    public render() {
        return (
            <div>
                <div className="row">
                    <div className="input-group">
                        <input className="form-control py-2 border-right-0 border" type="search" id="example-search-input" />
                        <span className="input-group-append">
                            <div className="input-group-text bg-transparent"><i className="fa fa-search"></i></div>
                        </span>
                    </div>
                </div>
                <RedditFeed />
            </div>
        )
    }
}
