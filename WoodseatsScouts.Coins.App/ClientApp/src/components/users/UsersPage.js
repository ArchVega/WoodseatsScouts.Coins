import React, {useEffect, useState} from "react";
import {Button, Col, Input, InputGroup, Row} from "reactstrap";
import EditUserPhotoModal from "./EditUserPhotoModal";
import UserPhotoModal from "./UserPhotoModal";

const UsersPage = () => {
    const [members, setMembers] = useState([]);
    const [userModal, setUserModal] = useState(false);
    const [editUserModal, setEditUserModal] = useState(false);
    const [selectedUser, setSelectedUser] = useState(null);
    const [filterStr, setFilterStr] = useState(null);
    const [showScores, setShowScores] = useState(false);

    useEffect(() => {
        const fetchMembers = async () => {
            const response = await fetch("home/GetMembersWithPoints");
            const members = await response.json();
            members.forEach(member => {
                switch (member.section) {
                    case "B":
                        member.section = "Beavers"
                        break;
                    case "C":
                        member.section = "Cubs"
                        break;
                    case "E":
                        member.section = "Explorers"
                        break;
                    case "A":
                        member.section = "Adults"
                        break;
                    case "S":
                        member.section = "Scouts"
                        break;
                }
            });
            setMembers(members);
        }
        fetchMembers().then();
    }, [])

    function showUserModal(user1) {
        setSelectedUser(user1)
        setUserModal(true)
    }

    function showEditUserModal(user1) {
        setSelectedUser(user1)
        setEditUserModal(true)
    }

    function getSectionClassName(section) {
        return `label-section label-section-${section.toLowerCase()}`
    }

    return <>
        <Row className="mb-3">
            <Col>
                <Input
                    className="filter-members"
                    autoFocus={true}
                    placeholder="Search to filter members, groups or section"
                    onChange={x => setFilterStr(x.target.value)}
                />
            </Col>
        </Row>
        <Row>
            <Col>
                <table className="table table-hover members-table">
                    <thead>
                    <tr>
                        <th className="fit"></th>
                        <th>NAME</th>
                        <th className="fit">GROUP</th>
                        <th className="fit">TOTAL POINTS
                            <input
                                type="checkbox"
                                onChange={(event) => setShowScores(event.target.checked)}
                                style={{marginLeft: '2px'}}/>
                        </th>
                        <th className="fit">SECTION</th>
                        <th className="fit">WRIST CODE</th>
                        <th className="fit">EDIT PHOTO</th>
                    </tr>
                    </thead>
                    <tbody>
                    {members
                        .filter(member => {
                            if (filterStr === null) {
                                // console.log(member, "filterStr === null")
                                return true;
                            }

                            if (filterStr != null && filterStr.trim().length === 0) {
                                // console.log(member, "filterStr != null && filterStr.trim().length === 0")
                                return true;
                            }

                            const firstNameMatch = member.firstName.toLowerCase().includes(filterStr.toLowerCase());
                            const lastNameMatch = (member.lastName !== null && member.lastName.toLowerCase().includes(filterStr.toLowerCase()));
                            const troopNameMatch = member.troopName.toString().toLowerCase().includes(filterStr.toLowerCase());
                            const sectionMatch = member.section.toString().toLowerCase().includes(filterStr.toLowerCase())
                            const memberCodeMatch = member.memberCode.toLowerCase().includes(filterStr.toLowerCase());
                            
                            // console.log(member, firstNameMatch, lastNameMatch, troopNameMatch, sectionMatch, memberCodeMatch);
                            
                            return firstNameMatch
                                || lastNameMatch
                                || troopNameMatch 
                                || sectionMatch
                                || memberCodeMatch
                        })
                        .map(x => (
                            <tr key={x.memberCode}>
                                <td>
                                <span className="show-user-photo-icon"
                                      onClick={() => showUserModal(x)}>
                                    <img key={Date.now()}
                                         src={x.hasImage ? `member-images/${x.id}.jpg?x=${new Date().getTime()}` : "images/unknown-member-image.png"}/>
                                </span>
                                </td>
                                <td><strong>{x.firstName + " " + x.lastName}</strong></td>
                                <td><span>{x.troopName}</span></td>
                                <td>
                                    <span>
                                        {showScores ? x.totalPoints : "xxx"}
                                    </span>
                                </td>
                                <td><strong className={getSectionClassName(x.section)}>{x.section}</strong></td>
                                <td><span>{x.memberCode}</span></td>
                                <td>
                                <span className="edit-user-photo-icon" onClick={() => showEditUserModal(x)}>
                                    ✎
                                </span>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </Col>
        </Row>

        <UserPhotoModal usersModal={userModal} setUsersModal={setUserModal} selectedUser={selectedUser}>
        </UserPhotoModal>
        <EditUserPhotoModal editUsersModal={editUserModal} setEditUsersModal={setEditUserModal}
                            selectedUser={selectedUser}>
        </EditUserPhotoModal>
    </>
}

export default UsersPage;