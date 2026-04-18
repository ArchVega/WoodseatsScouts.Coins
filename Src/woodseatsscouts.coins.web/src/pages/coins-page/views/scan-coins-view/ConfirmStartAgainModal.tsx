// import {Button, Col, Modal, ModalBody, ModalHeader, Row} from "reactstrap";
// import axios from "axios";
// import Uris from "../../../services/Uris";
//
// export default function ConfirmStartAgainModal({modal, setModal}) {
//   const toggleModal = () => setModal(!modal);
//
//   function confirmStartAgain() {
//     window.location.href = "/"
//   }
//
//   return (
//     <Modal isOpen={modal} toggle={toggleModal}>
//       <ModalHeader toggle={toggleModal}>Log out?</ModalHeader>
//       <ModalBody className="text-center">
//         <Row className="mb-3">
//           <Col>
//             <p>Are you sure?</p>
//           </Col>
//         </Row>
//         <Row className="start-over-modal-buttons">
//           <Col >
//             <Button className="btn btn-danger" onClick={confirmStartAgain} data-testid="button-confirm-start-again">
//               Yes
//             </Button>
//           </Col>
//           <Col>
//             <Button type="button" className="btn btn-success" onClick={() => setModal(false)}>
//               No
//             </Button>
//           </Col>
//         </Row>
//       </ModalBody>
//     </Modal>
//   )
// }