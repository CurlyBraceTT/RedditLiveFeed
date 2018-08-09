import * as React from 'react';
import { NavMenu } from 'components';

export interface LayoutProps {
    children?: React.ReactNode;
}

export class Layout extends React.Component<LayoutProps, {}> {
    public render() {
        return (
            <div>
                <NavMenu />
                <div className='container'>
                    {this.props.children}
                </div>
            </div>
        );
    }
}
