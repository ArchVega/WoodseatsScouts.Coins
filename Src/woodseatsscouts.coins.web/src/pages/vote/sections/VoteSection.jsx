import {Button, Col, Row} from "reactstrap";
import wave from "../../../images/wave.png";
import {useEffect, useState} from "react";
import axios from "axios";
import Uris from "../../../services/Uris";
import SectionNames from "../../homepage/sections/SectionNames";
import ConfirmVoteModal from "./ConfirmVoteModal";

export default function VoteSection({member, setVoteResult}) {
  const [countries, setCountries] = useState([])
  const [selectedCountry, setSelectedCountry] = useState(null)
  const [modal, setModal] = useState(false);

  useEffect(() => {
    axios
      .get(Uris.countries)
      .then(async response => {
        const data = await response.data;
        setCountries(data)
      })
  }, [])

  function renderCountry(country) {
    return (
      <div key={country.id} className="p-2 my-auto" style={{width: '20%'}}>
        <div className="country-div text-center border border-primary border-5"
             data-testid={`vote-for-${country.name.toLowerCase()}`}
             onClick={() => setSelectedCountry(country)}
             style={{height: '140px', position: "relative"}}>
          <img src={`images/countries/${country.name}.png`} style={{width: '100%', height: '100%'}} onClick={() => setModal(true)}/>
        </div>
      </div>
    )
  }

  return (
    <>
      <Row className="mb-4">
        <Col>
          <h1 id="scan-coins-section-h1" className="text-center">
            Hi&nbsp;<span className="font-black" data-testid="h1-user-firstname">{member.firstName}</span>
            <img id="scan-coins-section-wave-image" src={wave} alt=""/>
            <br/>
            <span>Which country do you want to vote for?</span>
          </h1>
        </Col>
      </Row>
      <Row>
        <Col>
          <div className="d-flex flex-wrap">
            {countries && countries.map(country => renderCountry(country))}
          </div>
        </Col>
      </Row>
      <ConfirmVoteModal member={member} country={selectedCountry} setVoteResult={setVoteResult} modal={modal} setModal={setModal}></ConfirmVoteModal>
    </>
  )
}