import * as React from 'react';
import RedditEntry from './RedditEntry';
import * as signalR from "@aspnet/signalr";
import {
    CSSTransition,
    TransitionGroup,
} from 'react-transition-group';

interface RedditFeedState {
    connection: any,
    entries: Array<any>,
}

export class RedditFeed extends React.Component<any, RedditFeedState> {
    state = {
        connection: null,
        entries: new Array<any>(),
    };

    constructor(props) {
        super(props);
    }

    componentWillUnmount() {
        this.state.connection = null;
    }

    componentDidMount() {
        const feedId = this.props.feedId;

        if (!feedId) {
            return;
        }

        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/hub")
            .build();

        connection.start()
            .then(() => {
                console.log('Connection started!')

                connection.stream("Feed", feedId)
                    .subscribe({
                        next: (feed) => {
                            console.log(feed);
                            const items = feed.concat(this.state.entries);
                            this.setState({ entries: items });
                        },
                        complete: () => {
                            console.error("complete");
                        },
                        error: (err) => {
                            console.error(err);
                        },
                    });

            })
            .catch(err => console.log('Error while establishing connection :('));
    }

    render() {
        var entriesJsx : any = '';

        if (this.state.entries != null) {
            entriesJsx = this.state.entries.map((entry : any) => {
                return (
                    <CSSTransition
                        key={entry.title}
                        timeout={200}
                        classNames="fade">
                        <RedditEntry key={entry.title} entry={entry} />
                    </CSSTransition>
                );
            });
        }

        return (
            <div className="row">
                <TransitionGroup className="reddit-entries"> 
                    {entriesJsx}
                </TransitionGroup>
            </div>
        );
    }
}

export default RedditFeed;