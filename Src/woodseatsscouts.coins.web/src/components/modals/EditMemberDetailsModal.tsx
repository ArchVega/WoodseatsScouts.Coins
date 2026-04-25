import "./EditMemberPhotoModal.scss"
import React, {useEffect, useState} from "react";
import {BaseModal} from "./BaseModal.tsx";
import Uris from "../../services/apis/Uris.ts";
import {Button} from "../widgets/HtmlControlWrappers.tsx";
import type {ActivityGroupDto, ScoutMemberCompleteDto, ScoutGroupDto, ScoutSectionDto} from "../../types/ServerTypes.ts";
import AppStateApiService from "../../services/apis/AppStateApiService.tsx";

interface EditMemberDetailsModalProps {
  showModal: boolean;
  setShowModal: React.Dispatch<React.SetStateAction<boolean>>
  memberCompleteDto: ScoutMemberCompleteDto;
  setMemberCompleteDto: React.Dispatch<React.SetStateAction<ScoutMemberCompleteDto>>
}

export default function EditMemberDetailsModal({showModal, setShowModal, memberCompleteDto, setMemberCompleteDto}: EditMemberDetailsModalProps) {
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [validationMessage, setValidationMessage] = useState("");

  const [scoutGroups, setScoutGroups] = useState<ScoutGroupDto[]>([]);
  const [scoutSections, setScoutSections] = useState<ScoutSectionDto[]>([]);

  const [selectedScoutGroupId, setSelectedScoutGroupId] = useState<number | undefined>(undefined);
  const [selectedScoutSectionCode, setSelectedScoutSectionCode] = useState<string | undefined>(undefined);

  useEffect(() => {
    if (memberCompleteDto) {
      setFirstName(memberCompleteDto.firstName);
      setLastName(memberCompleteDto.lastName);
      setSelectedScoutGroupId(memberCompleteDto.scoutGroupId)
      setSelectedScoutSectionCode(memberCompleteDto.scoutSectionCode)
    }

    AppStateApiService().getScoutGroups().then((response) => response.data).then((data) => setScoutGroups(data))
    AppStateApiService().getSections().then((response) => response.data).then((data) => setScoutSections(data))

  }, [memberCompleteDto]);

  useEffect(() => {
    const isFirstNameEmpty = firstName.length === 0
    const isLastNameEmpty = lastName.length === 0

    if (isFirstNameEmpty && isLastNameEmpty) {
      setValidationMessage("Please enter both a first and last name");
    } else if (isFirstNameEmpty) {
      setValidationMessage("Please enter a first name");
    } else if (isLastNameEmpty) {
      setValidationMessage("Please enter a last name");
    } else {
      setValidationMessage("")
    }
  }, [firstName, lastName]);

  function updateMemberName(e) {
    e.preventDefault();

    const payload = {
      firstName: firstName,
      lastName: lastName,
    }

    const requestOptions = {
      method: 'POST',
      headers: {'Content-Type': 'application/json'},
      body: JSON.stringify(payload)
    };

    fetch(Uris.scouts().members().memberName(memberCompleteDto.id), requestOptions).then((r) => {
      const updatedSelectedMember = ({...memberCompleteDto})
      updatedSelectedMember.firstName = firstName
      updatedSelectedMember.lastName = lastName
      updatedSelectedMember.fullName = `${firstName} ${lastName}`

      setMemberCompleteDto(updatedSelectedMember)
      setShowModal(false);
    });
  }

  return (
    <BaseModal
      id={"app-settings-modal"}
      title={`Update ${memberCompleteDto.firstName}'s details`}
      show={showModal}
      onClose={() => {
        setShowModal(false)
      }}>
      <form>
        <div className={"row mb-3"} style={{height: "50px"}}>
          <div className={"col"}>
            <div className={"row mb-1"}>
              <div className={"col-6"}>
                <input type="text" className="form-control textbox-thin-border" aria-describedby="firstName" placeholder={"First name"} value={firstName}
                       onChange={(e) => setFirstName(e.target.value)}/>
              </div>
              <div className={"col-6"}>
                <input type="text" className="form-control textbox-thin-border" aria-describedby="lastName" placeholder={"Last name"} value={lastName}
                       onChange={(e) => setLastName(e.target.value)}/>
              </div>
            </div>
            <div className={"row"}>
              <div className={"col"}>
                <p className={"text-danger text-sm-center small"}>
                  {validationMessage}
                </p>
              </div>
            </div>
          </div>
        </div>
        <div className={"row mb-3"}>
          <div className={"col"}>
            <select className={"form-select"} value={selectedScoutGroupId}>
              {scoutGroups && scoutGroups.map((scoutGroup) => (
                <option key={scoutGroup.id} value={scoutGroup.id}>{scoutGroup.name}</option>
              ))}
            </select>
          </div>
        </div>
        <div className={"row mb-3"}>
          <div className={"col"}>
            <select className={"form-select"} value={selectedScoutSectionCode}>
              {scoutSections && scoutSections.map((scoutSection) => (
                <option key={scoutSection.code} value={scoutSection.code}>{scoutSection.name}</option>
              ))}
            </select>
          </div>
        </div>
        <div className={"row"}>
          <div className={"col-12"}>
            <Button type={"submit"} onClick={(e) => updateMemberName(e)} className={"btn-success float-end w-25"} disabled={validationMessage.length > 0}>Ok</Button>
          </div>
        </div>
      </form>
    </BaseModal>
  )
}