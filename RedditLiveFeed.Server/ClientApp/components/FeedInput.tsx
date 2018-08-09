import * as React from 'react';
import {
    withRouter
} from "react-router-dom";

interface FeedInputState {
    subreddit: string
}

class FeedInputInner extends React.Component<any, FeedInputState>  {
    state = {
        subreddit: ''
    };

    constructor(props) {
        super(props);
        this.handleChange = this.handleChange.bind(this);
    }

    handleChange(event) {
        this.setState({ subreddit: event.target.value });
    }

    public render() {
        const url = '/r/' + this.state.subreddit;

        return (
            <div className="search-container input-group">
                <input placeholder="Enter subreddit name"
                    className="form-control py-2 border-right-0 border"
                    type="search" id="example-search-input"
                    value={this.state.subreddit} onChange={this.handleChange}
                />
                <span className="input-group-append">
                    <button className="btn btn-outline-secondary"
                        onClick={() => { this.props.history.push(url) }} type="button">
                        <i className="fa fa-chevron-right"></i>
                    </button>
                </span>
            </div>
        )
    }
}

const FeedInput = withRouter(FeedInputInner);
export { FeedInput };