import {Button, Col, Modal, ModalBody, ModalHeader, Row} from "reactstrap";
import axios from "axios";
import Uris from "../../../services/Uris";

export default function ConfirmVoteModal({country, member, setVoteResult, modal, setModal}) {
  const toggleModal = () => setModal(!modal);

  function confirmVote() {
    if (country) {
      axios
        .put(Uris.registerVoteForMember(member.memberId, country.id))
        .then(async response => {
          const data = await response.data;
          setVoteResult(data)
        })
    }
  }


  if (country) {
    return (
      <Modal isOpen={modal} toggle={toggleModal}>
        <ModalHeader toggle={toggleModal}>Vote for {country.name}?</ModalHeader>
        <ModalBody  className="text-center">
          <Row className="mb-3">
            <Col>
              <img src={`images/countries/${country.name}.png`} style={{width: '400px', height: '100%'}}/>
            </Col>
          </Row>
          <Row>
            <Col>
              <Button className="btn btn-success" onClick={confirmVote}>
                Confirm
              </Button>
            </Col>
          </Row>
        </ModalBody>
      </Modal>
    )
  }

  return null;
}