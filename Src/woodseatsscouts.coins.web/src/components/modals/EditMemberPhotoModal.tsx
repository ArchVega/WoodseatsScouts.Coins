import "./EditMemberPhotoModal.scss"
import Webcam from "react-webcam";
import React, {useCallback, useEffect, useRef, useState} from "react";
import {BaseModal} from "./BaseModal.tsx";
import Uris from "../../services/apis/Uris.ts";
import {Button} from "../widgets/HtmlControlWrappers.tsx";
import type {ScoutMemberCompleteDto} from "../../types/ServerTypes.ts";

interface EditMemberPhotoModalProps {
  showEditMemberPhotoModal: boolean;
  setShowEditMemberPhotoModal: React.Dispatch<React.SetStateAction<boolean>>
  memberCompleteDto: ScoutMemberCompleteDto;
  setMemberCompleteDto: React.Dispatch<React.SetStateAction<ScoutMemberCompleteDto>>
}

export default function EditMemberPhotoModal({showEditMemberPhotoModal, setShowEditMemberPhotoModal, memberCompleteDto, setMemberCompleteDto}: EditMemberPhotoModalProps) {
  const webcamRef = useRef<Webcam>(null);

  const [screenshot, setScreenshot] = useState<string | null>(null);

  const capture = useCallback(() => {
    if (webcamRef.current) {
      const screenshot = webcamRef.current.getScreenshot();
      if (screenshot) {
        setScreenshot(screenshot);
      }
    }
  }, [webcamRef]);

  function saveImage(base64Image: any) {
    if (!memberCompleteDto) {
      return
    }

    const payload = {
      photo: base64Image
    }

    const requestOptions = {
      method: 'PUT',
      headers: {'Content-Type': 'application/json'},
      body: JSON.stringify(payload)
    };

    fetch(Uris.scouts().members().updateMemberPhoto(memberCompleteDto.id), requestOptions).then(() => {
      const updatedSelectedMember = ({...memberCompleteDto})
      updatedSelectedMember.hasImage = true;
      setMemberCompleteDto(updatedSelectedMember)
      setShowEditMemberPhotoModal(false);
    });
  }

  useEffect(() => {
    if (screenshot) {
      saveImage(screenshot)
    }
  }, [screenshot]);

  const videoConstraints = {
    facingMode: "environment"
  };

  if (memberCompleteDto == null) {
    return null
  }

  return (
    <BaseModal
      id={"app-settings-modal"}
      title="Update participant's photo"
      show={showEditMemberPhotoModal}
      onClose={() => {
        setShowEditMemberPhotoModal(false)
      }}>
      <div className="row mb-3 mt-2">
        <div className="col text-center fs-3">
          Take a photo of <strong>{memberCompleteDto.firstName}</strong>
        </div>
      </div>
      <div className="row mb-2">
        <div className="col">
          <div className="webcam-wrapper">
            <Webcam
              id="webcam"
              width={"100%"}
              ref={webcamRef}
              audio={false}
              screenshotFormat="image/jpeg"
              forceScreenshotSourceSize={true}
              videoConstraints={videoConstraints}/>
            <div className="webcam-overlay" />
          </div>
        </div>
      </div>
      <Button className="btn btn-success" onClick={capture}>Capture photo</Button>
    </BaseModal>
  )
}