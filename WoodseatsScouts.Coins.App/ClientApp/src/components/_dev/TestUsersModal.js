import {Button, Col, Modal, ModalBody, ModalFooter, ModalHeader, Row} from "reactstrap";

const TestUsersModal = ({testUsersModal, setTestUsersModal, children}) => {
    const toggleTestUsersModal = () => setTestUsersModal(!testUsersModal);

    return <>
        <Modal isOpen={testUsersModal} toggle={toggleTestUsersModal}>
            <ModalHeader toggle={toggleTestUsersModal}>Test Users</ModalHeader>
            <ModalBody>
                <Row className="mb-3">
                    <Col><strong>Select a user</strong></Col>
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

export default TestUsersModal;