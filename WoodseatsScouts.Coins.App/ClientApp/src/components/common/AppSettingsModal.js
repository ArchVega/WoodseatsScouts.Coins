import {Button, Col, Form, FormGroup, Input, Label, Modal, ModalBody, ModalFooter, ModalHeader, Row} from "reactstrap";
import {useContext, useEffect} from "react";
import {AppCameraAvailableContext, UseAppCameraContext} from "../../App";

const AppSettingsModal = ({appSettingsModal, setAppSettingsModal}) => {
    const [useAppCamera, setUseAppCamera] = useContext(UseAppCameraContext)
    const [appCameraAvailable] = useContext(AppCameraAvailableContext)    
    const toggleModal = () => setAppSettingsModal(!appSettingsModal);

    useEffect(() => {
        const useCameraSetting = localStorage.getItem("woodseatsscouts.preferences.use-camera");
        const useCamera = useCameraSetting != null && useCameraSetting === "true"
        setUseAppCamera(useCamera)
    }, [])
    
    useEffect(() => {
        localStorage.setItem("woodseatsscouts.preferences.use-camera", useAppCamera);
    }, [useAppCamera])

    return <>
        <Modal isOpen={appSettingsModal} toggle={toggleModal}>
            <ModalHeader toggle={toggleModal}>
                Application settings
            </ModalHeader>
            <ModalBody>
                <Row className="mb-3">
                    <Col>
                            {appCameraAvailable ? <Form>
                                <Label>Use camera instead of usb scanner</Label>    
                                <FormGroup switch>
                                    <Input type="switch"
                                           role="switch"
                                           checked={useAppCamera}
                                           onChange={() => {
                                               setUseAppCamera(!useAppCamera);
                                           }}/>
                                </FormGroup>                                
                            </Form> : null}
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

export default AppSettingsModal;