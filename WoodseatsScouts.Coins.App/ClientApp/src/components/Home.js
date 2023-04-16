import React, {Component} from 'react';
import UserScanner from "./coins/userScanner";
import UserDetails from "./coins/userDetails";
import CoinScanner from "./coins/coinScanner";

export class Home extends Component {
    static displayName = Home.name;
    render() {
        let showUserDetailsAndCoinScanner = "visible";

        return (
            <div>
                <h3>COINS</h3>

                <div className="row">
                    <UserScanner></UserScanner>                    
                </div>

                <hr/>

                <div className="row" style={{display: showUserDetailsAndCoinScanner}}>
                    <div className="col-6">
                        <UserDetails></UserDetails>                        
                    </div>
                    <div className="col-6">
                        <CoinScanner></CoinScanner>
                    </div>
                </div>               
            </div>
        );
    }
}
