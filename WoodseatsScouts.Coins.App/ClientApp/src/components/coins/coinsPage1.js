import React, {Component, useEffect, useState} from 'react';
import UserScanner from "./UserScanner";
import UserDetails1 from "./UserDetails1";
import CoinScanner1 from "./CoinScanner1";

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
                    <UserScanner></UserScanner>
                </div>

                {user != null ? (
                    <>
                        <hr/>

                        <div className="row">
                            <div className="col-6">
                                <UserDetails1></UserDetails1>
                            </div>
                            <div className="col-6">
                                <CoinScanner1></CoinScanner1>
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
