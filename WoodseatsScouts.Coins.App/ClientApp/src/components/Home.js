import React, {Component} from 'react';
import UserScanner from "./coins/userScanner";
import UserDetails from "./coins/userDetails";
import CoinScanner from "./coins/coinScanner";

export class Home extends Component {
    static displayName = Home.name;

    render() {
        return (
            <div>
                <h3>COINS</h3>

                <UserScanner></UserScanner>

                <hr/>

                <div className="row">
                    <UserDetails></UserDetails>

                    <CoinScanner></CoinScanner>     
                </div>               
            </div>
        );
    }
}
