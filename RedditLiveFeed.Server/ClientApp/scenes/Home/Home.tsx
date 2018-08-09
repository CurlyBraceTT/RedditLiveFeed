import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { RedditFeed, FeedInput } from 'components';
import './Home.css';

export class Home extends React.Component<RouteComponentProps<any>, {}> {
    constructor(props) {
        super(props);
    }

    public render() {
        const feedId = this.props.match.params.feedId;

        return (
            <React.Fragment>
                <div className="row">
                    <div className="col-xl-4 offset-xl-4 col-lg-4 offset-lg-4 col-md-6 offset-md-3 col-sm-8 offset-sm-2 col-xs-12">
                        <FeedInput />
                    </div>
                </div>
                {feedId && <RedditFeed feedId={feedId} />}
            </React.Fragment>
        )
    }
}