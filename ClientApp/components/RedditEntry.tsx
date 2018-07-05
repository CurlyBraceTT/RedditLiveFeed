import * as React from 'react';

interface RedditEntryState {
    title: string,
}

class RedditEntry extends React.Component<any, RedditEntryState> {
    constructor(props: any) {
        super(props);
    }

    render() {
        const title = this.props.entry.title;
        const content = this.props.entry.content;

        return (
            <div>
                <h4>{title}</h4>
                <div dangerouslySetInnerHTML={{ __html: content }}></div>
            </div>
        )
    }
}

export default RedditEntry;