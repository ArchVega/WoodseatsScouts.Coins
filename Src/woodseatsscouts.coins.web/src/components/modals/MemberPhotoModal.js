import {Col, Modal, ModalBody, ModalHeader, Row} from "reactstrap";
import Uris from "../../services/Uris";

const MemberPhotoModal = ({usersModal, setUsersModal, selectedUser}) => {
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
                            <img src={selectedUser.hasImage
                                ? Uris.memberPhoto(selectedUser.id) // todo: rename to member
                                : "/images/unknown-member-image.png"}
                                 style={{maxWidth: '100%'}} alt=""/>

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

export default MemberPhotoModal;