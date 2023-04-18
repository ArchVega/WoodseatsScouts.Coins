import React, {useContext} from "react";
import {CoinPageCurrentUserContext} from "./CoinsPage1";

const UserDetails1 = () => {
    const [userQRCode, setUserQRCode, user, setUser] = useContext(CoinPageCurrentUserContext);

    let userName = user == null
        ? ""
        : user.scoutName;
    let scoutPhotoPath = user == null
        ? "/images/unknown-scout-image.jpg"
        : user.scoutPhotoPath;

    return <>
        <h4>{userName}</h4>
        <span id="scoutTroopNumberAndSection"></span>
        <img id="scout-photo" src={scoutPhotoPath} alt="Scout Photo"/>
    </>
}

export default UserDetails1;