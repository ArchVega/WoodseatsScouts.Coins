import {Button, Col, Modal, ModalBody, ModalFooter, ModalHeader, Row} from "reactstrap";

const TestCoinsModal = ({testCoinsModal, setTestCoinsModal, children}) => {
    const toggleTestUsersModal = () => setTestCoinsModal(!testCoinsModal);

    return <>
        <Modal isOpen={testCoinsModal} toggle={toggleTestUsersModal}>
            <ModalHeader toggle={toggleTestUsersModal}>Test Coins</ModalHeader>
            <ModalBody>
                <Row className="mb-3">
                    <Col><strong>Select a coin</strong></Col>
                </Row>
                <Row>
                    <Col>
                        {children}
                    </Col>
                </Row>
            </ModalBody>
        </Modal>
    </>
}

export default TestCoinsModal;