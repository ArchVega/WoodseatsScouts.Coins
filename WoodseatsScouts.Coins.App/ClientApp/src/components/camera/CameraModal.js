import {Button, Col, Modal, ModalBody, ModalFooter, ModalHeader, Row} from "reactstrap";
import {useEffect, useState} from "react";
import {QRCodeScanner} from "./QRCodeScanner";

const CameraModal = ({title, userQRCode, setUserQRCode, cameraModal, setCameraModal}) => {
    const toggleCameraModal = () => setCameraModal(!cameraModal);
    
    const  [qrCode, setQRCode] = useState("")
    
    useEffect(() => {
        setCameraModal(false);        
    }, [userQRCode])

    return <>
        <Modal isOpen={cameraModal} toggle={toggleCameraModal}>
            <ModalHeader toggle={toggleCameraModal}>{title}</ModalHeader>
            <ModalBody>
                <Row>
                    <Col>
                        <QRCodeScanner videoSizeEm={20} qrCode={userQRCode} setQRCode={setUserQRCode}/>
                    </Col>
                </Row>
            </ModalBody>
        </Modal>
    </>
}

export default CameraModal;