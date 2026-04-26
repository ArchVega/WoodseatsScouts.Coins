import "./EditMemberPhotoModal.scss"
import Webcam from "react-webcam";
import React, {useCallback, useEffect, useRef, useState} from "react";
import {BaseModal} from "./BaseModal.tsx";
import Uris from "../../services/apis/Uris.ts";
import {Button} from "../widgets/HtmlControlWrappers.tsx";
import type {ScoutMemberCompleteDto} from "../../types/ServerTypes.ts";
import {apiClient} from "../../services/apis/apiClient.ts";

interface EditMemberPhotoModalProps {
  showEditMemberPhotoModal: boolean;
  setShowEditMemberPhotoModal: React.Dispatch<React.SetStateAction<boolean>>
  memberCompleteDto: ScoutMemberCompleteDto;
  setMemberCompleteDto: React.Dispatch<React.SetStateAction<ScoutMemberCompleteDto>>
}

export default function EditMemberPhotoModal({showEditMemberPhotoModal, setShowEditMemberPhotoModal, memberCompleteDto, setMemberCompleteDto}: EditMemberPhotoModalProps) {
  const webcamRef = useRef<Webcam>(null);

  const [screenshot, setScreenshot] = useState<string | null>(null);
  const [startCamera, setStartCamera] = useState(false);

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

    apiClient.put(Uris.scouts().members().updateMemberPhoto(memberCompleteDto.id), payload).then(() => {
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
      }}
      isPhotoModal={true}>

      {!startCamera && (
        <div className="row">
          <div className="col text-center">
            <div className="fs-4 mb-5 mt-5">Click the button to allow the device's webcam to take photos.</div>
            <button className="fs-3 btn btn-success mb-5" onClick={() => setStartCamera(true)}>
              Start Camera
            </button>
          </div>
        </div>
      )}

      {startCamera && (
        <>
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
                  playsInline
                  onUserMediaError={(err) => console.log("camera error", err)}
                  audio={false}
                  screenshotFormat="image/jpeg"
                  forceScreenshotSourceSize={true}
                  videoConstraints={videoConstraints}/>
                <div className="webcam-overlay"/>
              </div>
            </div>
          </div>
          <Button className="fs-1 btn btn-success w-100" style={{height: "100px"}} onClick={capture}>Take Photo</Button>
        </>
      )}
    </BaseModal>
  )
}