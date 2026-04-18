import './MemberDetailsPage.scss'

import {useParams} from "react-router-dom";
import {useContext, useEffect, useState} from "react";
import {UseAppCameraContext} from "../../contexts/AppContextExporter.tsx";
import MemberApiService from "../../services/MemberApiService.ts";

function MemberDetailsPage() {
    const {useAppCamera} = useContext(UseAppCameraContext)
    const {memberCode} = useParams();
    const [member, setMember] = useState(null);
    const [selectedSession, setSelectedSession] = useState(null);
    const [userModal, setUserModal] = useState(false);
    const [editUserModal, setEditUserModal] = useState(false);
    const [editMemberNameModal, setEditMemberNameModal] = useState(false);
    const [selectedUser, setSelectedUser] = useState(null);
    const [filterText, setFilterText] = useState(null);
    const [showScores, setShowScores] = useState(true);

    // showMemberNameModal(member)}

    // todo: these are what we need to hook up
    // <EditMemberNameModal editMembersModal={editMemberNameModal} setEditMembersModal={setEditMemberNameModal} selectedMember={selectedUser}
    //                      setSelectedMember={setSelectedUser}/>
    // <EditMemberPhotoModal editUsersModal={editUserModal} setEditUsersModal={setEditUserModal} selectedUser={selectedUser}
    //                       setSelectedUser={setSelectedUser}/>

    useEffect(() => {
        if (memberCode) {
            MemberApiService().fetchMember(memberCode).then(response => response.data).then(member => setMember(member));
        }
    }, [memberCode])

    useEffect(() => {
        console.log('member', member);
    }, [member])

    function RenderMember(member) {
        return (
            <div className={"card member-details-member-card flex-shrink-0 sticky-top"}>
                <div className={"card-body"}>
                    <div className={"row"}>
                        <div className={"col"}>
                            {/*<img key={Date.now()}*/}
                            {/*     onClick={() => useAppCamera ? showEditUserModal(member) : alert('Device does not have a camera or it is unavailable.')}*/}
                            {/*     title={"User id: " + member.id}*/}
                            {/*     src={member.hasImage ? Uris.memberPhoto(member.id) : "images/unknown-member-image.png"} alt=""/>*/}
                        </div>
                    </div>
                    <div className={"row"}>
                        <div className={"d-flex justify-content-center align-items-center members-list-item-section"} style={{height: "100px"}}>
                            <strong className={"d-flex flex-column justify-content-center h-100"}>{member.firstName + " " + member.lastName}</strong>
                        </div>
                    </div>
                    <div className="row g-1">
                        <div className={"col-7 members-list-item-section"}>
                            <div>
                                {member.memberCode}
                            </div>
                        </div>
                        <div className={"col-5 members-list-item-section"}>
                            <div>
                                {member.totalPoints}
                            </div>
                        </div>
                    </div>
                    <div className={"row"}>
                        <div className={"members-list-item-section"}>
                            <div>{member.troopName}</div>
                        </div>
                    </div>
                    <div className="row mb-3 g-1">
                        <div className={"members-list-item-section"}>
                            <button className={"text-black"}>Add fixed amount points</button>
                        </div>
                        <div className={"members-list-item-section"}>
                            <button className={"text-black"}>Remove fixed amount points</button>
                        </div>
                    </div>
                    <div className="row mb-3 g-1">
                        <div className={"members-list-item-section"}>
                            <button className={"text-black"}>Change Group</button>
                        </div>
                        <div className={"members-list-item-section"}>
                            <button className={"text-black"}>Change Section</button>
                        </div>
                    </div>
                </div>
            </div>
        )
    }

    const rows = Array.from({ length: 200 }); // 👈 controls height

    function RenderMemberScanSessions() {
        return (
            <table className="table table-striped">
                <thead>
                <tr>
                    <th>Time</th>
                    <th>Total Points</th>
                    <th>Edit</th>
                    <th>Delete</th>
                </tr>
                </thead>

                <tbody>
                {rows.map((_, i) => (
                    <tr key={i}>
                        <td>{new Date().toLocaleString()}</td>
                        <td>{i + 1}</td>
                        <td><button className={"btn-warning"}>EDIT</button></td>
                        <td><button className={"btn-danger"}>DELETE</button></td>
                    </tr>
                ))}
                </tbody>
            </table>
        )
    }

    function RenderSelectedScanSessions() {
        return (
            <table className="table table-striped">
                <thead>
                <tr>
                    <th>Base</th>
                    <th>Total Points</th>
                    <th>Edit</th>
                    <th>Delete</th>
                </tr>
                </thead>

                <tbody>
                {rows.map((_, i) => (
                    <tr key={i}>
                        <td>{['Archery', 'Shooting', "Cave Bus", "Arts and Crafts"][Math.floor(Math.random() * 4)]}</td>
                        <td>{i + 1}</td>
                        <td><button className={"btn-warning"}>EDIT</button></td>
                        <td><button className={"btn-danger"}>DELETE</button></td>
                    </tr>
                ))}
                </tbody>
            </table>
        )
    }

    if (member) {
        return (
            <div id={"member-details-page"}>
                <div className="row g-3">
                    <div className={"col-2"}>
                        {RenderMember(member)}
                    </div>
                    <div className={"col-4"}>
                        <div className={"card"}>
                            {RenderMemberScanSessions()}
                        </div>
                    </div>
                    <div className={"col-4"}>
                        <div className={"card"}>
                            {!selectedSession && (
                                <>Select a session</>
                            )}
                            {
                                RenderSelectedScanSessions()
                            }

                        </div>
                    </div>
                    <div className={"col-2"}>
                        <div className="row g-1 sticky-top">
                            <div className={"col-12"} >
                                <div className={"card"}>
                                    <div className={"card-body"}>
                                        <h5>Most Visited Base</h5>
                                        <div>Archery</div>
                                        <div>54 visits</div>
                                    </div>
                                </div>
                            </div>
                            {/*<div className={"col-12"}>*/}
                            {/*    <Card>*/}
                            {/*        <CardBody>*/}
                            {/*            <h5>Least Visited Base</h5>*/}
                            {/*            <div>229th Greenhill</div>*/}
                            {/*            <div>2 Visits</div>*/}
                            {/*        </CardBody>*/}
                            {/*    </Card>*/}
                            {/*</Col>*/}
                            {/*<div className={"col-12"}>*/}
                            {/*    <Card>*/}
                            {/*        <CardBody>*/}
                            {/*            <h5>Most Scans</h5>*/}
                            {/*            <div>44 Tokens</div>*/}
                            {/*        </CardBody>*/}
                            {/*    </Card>*/}
                            {/*</Col>*/}
                            {/*<Col xs={12}>*/}
                            {/*    <Card>*/}
                            {/*        <CardBody>*/}
                            {/*            <h5>Total tokens scanned</h5>*/}
                            {/*            <div>412</div>*/}
                            {/*        </CardBody>*/}
                            {/*    </Card>*/}
                            {/*</Col>*/}
                            {/*<Col xs={12}>*/}
                            {/*    <Button className={"btn-success"}>Save Changes</Button>*/}
                            {/*</Col>*/}
                        </div>
                    </div>
                </div>
            </div>
        )
    }

    return null
}

export default MemberDetailsPage;
