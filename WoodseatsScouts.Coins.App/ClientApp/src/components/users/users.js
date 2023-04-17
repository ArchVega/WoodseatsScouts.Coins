import React from "react";
import {Button, Input, InputGroup} from "reactstrap";

const UsersPage = () => {
    return <>
        <h3>Users</h3>
        <table className="table">
            <thead>
            <tr>
                <th>Name</th>
                <th>Photo</th>
            </tr>
            </thead>
            <tbody>
            <tr>
                <td>
                    Bob
                </td>
                <td>
                    <img src="/scout-images/1.jpg"/>
                </td>
            </tr>
            <tr>
                <td>
                    Carl
                </td>
                <td>
                    <img src="/scout-images/5.jpg"/>
                </td>
            </tr>
            </tbody>
        </table>
    </>
}

export default UsersPage;