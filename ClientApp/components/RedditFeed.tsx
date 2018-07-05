import * as React from 'react';
import RedditEntry from './RedditEntry';
import * as signalR from "@aspnet/signalr";
import {
    CSSTransition,
    TransitionGroup,
} from 'react-transition-group';

interface RedditFeedState {
    seconds: number,
    interval: number,
    connection: any,
    entries: any,
}

class RedditFeed extends React.Component<any, RedditFeedState> {
    state = {
        seconds: 0,
        interval: 0,
        connection : null,
        entries: null,
    };

    constructor() {
        super();
    }

    tick() {
        //this.setState((prevState: RedditFeedState) => ({
        //    seconds: prevState.seconds + 1
        //}));
    }

    componentDidMount() {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/hub")
            .build();

        connection.start()
            .then(() => console.log('Connection started!'))
            .catch(err => console.log('Error while establishing connection :('));

        connection.on("RefreshFeed", (feedStr: string) => {
            const feed = JSON.parse(feedStr);
            console.log(feed);
            this.setState({ entries: feed.entries });
        });

        //const interval = setInterval(() => this.tick(), 1000);
        //this.setState({ interval: interval });
    }

    componentWillUnmount() {
        clearInterval(this.state.interval);
    }

    render() {
        var entries = '';

        if (this.state.entries != null) {
            entries = this.state.entries.map((entry : any) => {
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
            <div className="container">
                <TransitionGroup className="todo-list"> 
                    {entries}
                </TransitionGroup>
            </div>
        );
    }
}

export default RedditFeed;