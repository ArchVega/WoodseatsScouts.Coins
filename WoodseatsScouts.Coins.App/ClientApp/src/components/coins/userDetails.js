import React from "react";

const user = {
    name: "TestUser",
    image: "/scout-images/1.jpg"
}

const UserDetails = () => {
    return  <>
        <h4>{user.name}</h4>
        <span id="scoutTroopNumberAndSection"></span>
        <img id="scout-photo" src={user.image} alt="Scout Photo"/>
    </>
}

export default UserDetails;