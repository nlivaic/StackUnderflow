import React, { Component } from 'react';

class Error extends Component {
    constructor(props) {
        super(props);
        this.state = { hasError: false };
    }

    componentDidCatch(error, info) {
        this.setState({ hasError: true });
    }

    render() {
        if (this.state.hasError) {
            return (
                <div>
                    <h1>Sorry, an error happened. We are on it.</h1>
                </div>
            );
        }
        return this.props.children;
    }
}

export default Error;