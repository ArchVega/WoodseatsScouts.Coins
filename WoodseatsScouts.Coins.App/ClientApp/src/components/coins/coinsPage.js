import React, {Component} from 'react';
import UserScanner from "./UserScanner";
import UserDetails from "./userDetails";
import CoinScanner from "./coinScanner";

export class CoinsPage extends Component {
    static displayName = CoinsPage.name;
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
