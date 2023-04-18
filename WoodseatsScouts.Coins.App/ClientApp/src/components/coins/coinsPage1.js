import React, {Component, useEffect, useState} from 'react';
import UserScanner from "./UserScanner";
import UserDetails from "./userDetails";
import CoinScanner from "./coinScanner";

const CoinPageCurrentUserContext = React.createContext(null);

const CoinsPage1 = () => {
    const [userQRCode, setUserQRCode] = useState(null)
    const [user, setUser] = useState(null)

    useEffect(() => {
        const fetchUser = async () => {
            if (userQRCode != null) {
                const response = await fetch("home/GetScoutInfoFromCode?code=" + userQRCode);
                const user = await response.json();
                setUser(user);
            }
        }
        fetchUser().then();
    }, [userQRCode])

    return (
        <CoinPageCurrentUserContext.Provider value={[userQRCode, setUserQRCode, user, setUser]}>
            <div>
                <h3>COINS</h3>

                <div className="row">
                    <UserScanner setUserQRCode={{user, setUserQRCode}}></UserScanner>
                </div>

                {user != null ? (
                    <>
                        <hr/>

                        <div className="row">
                            <div className="col-6">
                                <UserDetails></UserDetails>
                            </div>
                            <div className="col-6">
                                <CoinScanner></CoinScanner>
                            </div>
                        </div>
                    </>
                ) : null}
            </div>
        </CoinPageCurrentUserContext.Provider>
    );
}

export {CoinPageCurrentUserContext};

export default CoinsPage1;
