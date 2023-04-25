import {Button, Col, Modal, ModalBody, ModalFooter, ModalHeader, Row} from "reactstrap";

const UserPhotoModal = ({usersModal, setUsersModal, selectedUser}) => {
    const toggleUserModal = () => setUsersModal(!usersModal);
    
    return <>
        <Modal isOpen={usersModal} toggle={toggleUserModal}>
            <ModalHeader toggle={toggleUserModal}>
                {selectedUser == null ? "" : `${selectedUser.firstName} ${selectedUser.lastName} (${selectedUser.memberCode})`}
            </ModalHeader>
            <ModalBody>
                <Row className="mb-3">
                    <Col>
                        {selectedUser != null ? (
                            <img
                                src={selectedUser.hasImage ? `member-images/${selectedUser.id}.jpg?x=${new Date().getTime()}` : "images/unknown-member-image.png"}
                                style={{maxWidth: '100%'}}/>

                        ) : null}
                    </Col>
                </Row>
                <Row>
                    <Col>
                    </Col>
                </Row>
            </ModalBody>
        </Modal>
    </>
}

export default UserPhotoModal;