import {Button, Col, Modal, ModalBody, ModalFooter, ModalHeader, Row} from "reactstrap";
import Webcam from "react-webcam";
import {useContext, useRef} from "react";
import {AppModeContext} from "../../App";
import Uris from "../../services/Uris";

const EditMemberPhotoModal = ({editUsersModal, setEditUsersModal, selectedUser}) => {
    const toggleEditUserModal = () => setEditUsersModal(!editUsersModal);

    function saveImage(base64Image) {
        const payload = {
            photo: base64Image
        }

        const requestOptions = {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify(payload)
        };

        fetch( Uris.memberPhoto(selectedUser.id), requestOptions);
        setEditUsersModal(false);
    }

    const videoConstraints = {
        facingMode: "environment"
    };

    return <>
        {selectedUser == null ? null : (
            <Modal isOpen={editUsersModal} toggle={toggleEditUserModal}>
                <ModalHeader toggle={toggleEditUserModal}>
                    {selectedUser.firstName + " " + selectedUser.lastName}
                </ModalHeader>

                <ModalBody>
                    <Row className="mb-3">
                        <Col><strong>Take a photo
                            of {selectedUser.firstName + " " + selectedUser.lastName}</strong></Col>
                    </Row>
                    <Row>
                        <Col>
                        </Col>
                    </Row>
                    <Row>
                        <Col className="photo-webcam">
                            <Webcam
                                audio={false}
                                screenshotFormat="image/jpeg"
                                forceScreenshotSourceSize={true}
                                videoConstraints={videoConstraints}>
                                {({getScreenshot}) => (
                                    <button className="btn btn-success"
                                            onClick={() => {
                                                const imageSrc = getScreenshot()
                                                saveImage(imageSrc)
                                            }}>
                                        Capture photo
                                    </button>
                                )}
                            </Webcam>
                        </Col>
                    </Row>
                </ModalBody>
            </Modal>
        )}
    < />
}

export default EditMemberPhotoModal