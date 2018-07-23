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
        const previewUrl = this.props.entry.thumbnail;
        const subreddit = this.props.entry.subreddit;
        const url = this.props.entry.url;
        const entryRedditUrl = this.props.entry.entryRedditUrl;
        const subredditUrl = this.props.entry.entryRedditUrl;
        const created = new Date(this.props.entry.createdParsed).toLocaleTimeString();

        return (
            <div className="block">
                {previewUrl.startsWith("http") &&
                    <a target="_blank" href={url}>
                        <img src={previewUrl} className="image-preview" alt="Preview" />
                    </a>
                }
                <div className="block-text">
                    <h4><a target="_blank" className="entry-url" href={entryRedditUrl}>{title}</a></h4>
                    <span>submitted at {created} to <a target="_blank" href={subredditUrl}>{subreddit}</a></span>
                </div>
            </div>
        )
    }
}

export default RedditEntry;