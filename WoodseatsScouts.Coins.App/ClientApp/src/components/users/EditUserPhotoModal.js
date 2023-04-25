import {Button, Col, Modal, ModalBody, ModalFooter, ModalHeader, Row} from "reactstrap";
import Webcam from "react-webcam";

const EditUserPhotoModal = ({editUsersModal, setEditUsersModal, selectedUser}) => {
    const toggleEditUserModal = () => setEditUsersModal(!editUsersModal);

    function saveImage(base64Image) {
        console.log(base64Image, selectedUser.id)

        const payload = {
            memberId: selectedUser.id,
            photo: base64Image
        }

        const requestOptions = {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify(payload)
        };
        fetch('home/SaveMemberPhoto', requestOptions);
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
                                {({ getScreenshot }) => (
                                    <button className="btn btn-success"
                                        onClick={() => {
                                            const imageSrc = getScreenshot()
                                            saveImage(imageSrc)
                                        }}
                                    >
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

export default EditUserPhotoModal