import {Button, Col, Modal, ModalBody, ModalHeader, Row} from "reactstrap";
import Uris from "../../services/Uris";
import {useEffect, useState} from "react";

const EditMemberNameModal = ({editMembersModal, setEditMembersModal, selectedMember, setSelectedMember}) => {
    const toggleEditMemberModal = () => setEditMembersModal(!editMembersModal);

    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [validationMessage, setValidationMessage] = useState("");

    useEffect(() => {
        if (selectedMember) {
            setFirstName(selectedMember.firstName);
            setLastName(selectedMember.lastName);
        }
    }, [selectedMember]);

    useEffect(() => {
        const isFirstNameEmpty = firstName.length === 0
        const isLastNameEmpty = lastName.length === 0

        if (isFirstNameEmpty && isLastNameEmpty) {
            setValidationMessage("Please enter both a first and last name");
        } else if (isFirstNameEmpty) {
            setValidationMessage("Please enter a first name");
        } else if (isLastNameEmpty) {
            setValidationMessage("Please enter a last name");
        } else {
            setValidationMessage("")
        }
    }, [firstName, lastName]);

    function updateMemberName(e) {
        e.preventDefault();

        if (!selectedMember) {
            return
        }

        const payload = {
            firstName: firstName,
            lastName: lastName,
        }

        const requestOptions = {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify(payload)
        };

        fetch(Uris.memberName(selectedMember.id), requestOptions).then((r) => {
            const updatedSelectedMember = ({...selectedMember})
            updatedSelectedMember.firstName = firstName
            updatedSelectedMember.lastName = lastName
            updatedSelectedMember.fullName = `${firstName} ${lastName}`

            setSelectedMember(updatedSelectedMember)
            setEditMembersModal(false);
        });
    }

    return <>
        <Modal isOpen={editMembersModal} toggle={toggleEditMemberModal}>
            <ModalHeader toggle={toggleEditMemberModal}>
                Change your name
            </ModalHeader>

            <ModalBody>
                <form>
                    <Row className={"mb-3"} style={{height: "50px"}}>
                        <Col>
                            <Row className={"mb-1"}>
                                <Col className={"col-6"}>
                                    <input type="text" className="form-control textbox-thin-border" aria-describedby="firstName" placeholder={"First name"} value={firstName}
                                           onChange={(e) => setFirstName(e.target.value)}/>
                                </Col>
                                <Col className={"col-6"}>
                                    <input type="text" className="form-control textbox-thin-border" aria-describedby="lastName" placeholder={"Last name"} value={lastName}
                                           onChange={(e) => setLastName(e.target.value)}/>
                                </Col>
                            </Row>
                            <Row>
                                <Col>
                                    <p className={"text-danger text-sm-center small"}>
                                        {validationMessage}
                                    </p>
                                </Col>
                            </Row>
                        </Col>
                    </Row>
                    <Row>
                        <Col className={"col-12"}>
                            <Button type={"submit"} onClick={(e) => updateMemberName(e)} className={"btn-success float-end w-25"} disabled={validationMessage.length > 0}>Ok</Button>
                        </Col>
                    </Row>
                </form>
            </ModalBody>
        </Modal>
    < />
}

export default EditMemberNameModal