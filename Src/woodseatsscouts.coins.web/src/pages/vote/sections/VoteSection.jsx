import {Button, Col, Row} from "reactstrap";
import wave from "../../../images/wave.png";
import {useEffect, useState} from "react";
import axios from "axios";
import Uris from "../../../services/Uris";
import SectionNames from "../../homepage/sections/SectionNames";

export default function VoteSection({member, setVoteResult}) {
  const [countries, setCountries] = useState([])
  const [selectedCountry, setSelectedCountry] = useState(null)

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
             onClick={() => setSelectedCountry(country)}
             style={{height: '140px', position: "relative"}}>
          <img src="images/fictional-country-flag.png" style={{width: '100%', height: '100%'}}/>
          <div className="text-black bg-white"
               style={{position: "absolute", top: "50%", left: "50%", transform: "translate(-50%, -50%)", fontSize: "2.5em"}}>
            {country.name}
          </div>
        </div>
      </div>
    )
  }

  function confirmVote() {
    if (selectedCountry) {
      axios
        .put(Uris.registerVoteForMember(member.memberId, selectedCountry.id))
        .then(async response=> {
          const data = await response.data;
          setVoteResult(data)
        })
    }
  }

  return (
    <>
      <Row className="mb-4">
        <Col>
          <h1 id="scan-coins-section-h1">Hi&nbsp;<span className="font-black" data-testid="h1-user-firstname">{member.firstName}</span>
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
      <Row className="mt-5">
        <Col className="text-center">
          {selectedCountry
            ?
            <Button className="btn-success p-5 border border-primary border-1" style={{fontSize: "2em", width: '500px'}} onClick={confirmVote}>
              <div>Vote for <span className="text-white">{selectedCountry.name}</span></div>
              <div>
                <img src="images/fictional-country-flag.png" style={{width: '100%', height: '100%'}}/>
              </div>
            </Button>
            : null}
        </Col>
      </Row>
    </>
  )
}