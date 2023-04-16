import React from "react";

const user = {
    name: "TestUser",
    image: "/scout-images/1.jpg"
}

const UserDetails = () => {
    return  <div className="col-6">
        <h4>{user.name}</h4>
        <span id="scoutTroopNumberAndSection"></span>
        <img id="scout-photo" src={user.image} alt="Scout Photo"/>
    </div>
}

export default UserDetails;