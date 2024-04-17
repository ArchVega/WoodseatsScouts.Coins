import {Button, Col, Modal, ModalBody, ModalFooter, ModalHeader, Row} from "reactstrap";
import Webcam from "react-webcam";
import {useContext, useEffect, useRef, useState} from "react";
import {AppModeContext} from "../../App";
import Uris from "../../services/Uris";

const EditMemberPhotoModal = ({editUsersModal, setEditUsersModal, selectedUser, setSelectedUser}) => {
  const toggleEditUserModal = () => setEditUsersModal(!editUsersModal);

  const [overlay, setOverlay] = useState(null);
  const [hasOverlay, setHasOverlay] = useState(false);

  useEffect(() => {
    setHasOverlay(false)

    setTimeout(() => {
      const svgIcon = (
        <svg
          width="470px"
          height="350px"
          className="svg"
          viewBox="0 0 260 200"
          version="1.1"
          xmlns="http://www.w3.org/2000/svg"
          xmlnsXlink="http://www.w3.org/1999/xlink">
          <defs>
            <mask id="overlay-mask" x="0" y="0" width="100%" height="100%">
              <rect x="0" y="0" width="100%" height="100%" fill="#fff"/>
              <circle cx="50%" cy="50%" r="90" />
            </mask>
          </defs>
          <rect x="0" y="0" width="100%" height="100%" mask="url(#overlay-mask)" fillOpacity="0.3"/>
        </svg>
      );
      setOverlay(svgIcon)
      setHasOverlay(true)
    }, 2000)
  }, [editUsersModal])

  function saveImage(base64Image) {
    const payload = {
      photo: base64Image
    }

    const requestOptions = {
      method: 'PUT',
      headers: {'Content-Type': 'application/json'},
      body: JSON.stringify(payload)
    };

    fetch(Uris.memberPhoto(selectedUser.id), requestOptions).then(() => {
      const updatedSelectedMember = ({...selectedUser})
      updatedSelectedMember.hasImage = true;
      setSelectedUser(updatedSelectedMember)
      setEditUsersModal(false);
    });
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
            <Col className="photo-webcam webcam-container ">
              <Webcam
                id="webcam"
                audio={false}
                screenshotFormat="image/jpeg"
                forceScreenshotSourceSize={true}
                videoConstraints={videoConstraints}>
                {({getScreenshot}) => (
                  <>
                    <button className="btn btn-success mt-2"
                            onClick={() => {
                              const imageSrc = getScreenshot()
                              saveImage(imageSrc)
                            }}>
                      Capture photo
                    </button>
                  </>
                )}
              </Webcam>
              <div className="overlay-container">
                {hasOverlay ? overlay : null}
              </div>
            </Col>
          </Row>
        </ModalBody>
      </Modal>
    )}
  < />
}

export default EditMemberPhotoModal