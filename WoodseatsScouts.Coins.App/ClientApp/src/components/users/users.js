import React from "react";

const UsersPage = () => {
    return <>
        <h3>Users</h3>

        <div className="row mb-5">
            <div className="col-12">
                <div className="row g-3 align-items-center">
                    <div className="col-auto mb-3">
                        <label htmlFor="newUserName" className="col-form-label">Name</label>
                    </div>
                    <div className="col-5">
                        <input type="text" id="newUserName" className="form-control"
                               aria-describedby="" />
                    </div>
                    <div className="col-auto">
                        <button className="btn btn-primary">&#128247;</button>
                    </div>
                    <div className="col-auto">
                        <button className="btn btn-primary">&#128193;</button>
                    </div>
                </div>
                <div className="row g-3 align-items-center">
                    <div className="col-auto">
                        <img src="/images/unknown-scout-image.jpg" style={{width: "100px"}}/>
                    </div>
                    <div className="col-lg">
                        <button className="btn btn-success float-end">Add user</button>
                    </div>
                </div>
            </div>    
        </div>
        
        
        <hr/>

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