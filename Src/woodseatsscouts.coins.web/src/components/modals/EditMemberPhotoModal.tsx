import "./EditMemberPhotoModal.scss"
import Webcam from "react-webcam";
import React, {useCallback, useContext, useEffect, useRef, useState} from "react";
import {BaseModal} from "./BaseModal.tsx";
import Uris from "../../services/apis/Uris.ts";
import {Button} from "../widgets/HtmlControlWrappers.tsx";

export default function EditMemberPhotoModal({editUsersModal, setEditUsersModal, selectedUser, setSelectedUser}) {
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
    if (!selectedUser) {
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

    fetch(Uris.memberPhoto(selectedUser.id), requestOptions).then(() => {
      const updatedSelectedMember = ({...selectedUser})
      updatedSelectedMember.hasImage = true;
      setSelectedUser(updatedSelectedMember)
      setEditUsersModal(false);
    });
  }

  useEffect(() => {
    saveImage(screenshot)
  }, [screenshot]);

  const videoConstraints = {
    facingMode: "environment"
  };

  if (selectedUser == null) {
    return null
  }

  return (
    <BaseModal
      id={"app-settings-modal"}
      title="Update participant's photo"
      show={editUsersModal}
      onClose={() => {
        setEditUsersModal(false)
      }}>
      <div className="row mb-3 mt-2">
        <div className="col text-center fs-3">
          Take a photo of <strong>{selectedUser.firstName}</strong>
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

{/*{imageSrc && (*/
}
{/*  <div>*/
}
{/*    <h2>Captured Image:</h2>*/
}
{/*    <img src={imageSrc} alt="Captured"/>*/
}
{/*  </div>*/
}
{/*)}*/
}

{/*<div className="overlay-container">*/
}
{/*  {hasOverlay ? overlay : null}*/
}
{/*</div>*/
}