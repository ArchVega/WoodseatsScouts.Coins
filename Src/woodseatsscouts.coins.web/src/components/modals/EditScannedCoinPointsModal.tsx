import React, {useContext, useEffect, useState} from "react";
import {AppCameraAvailableContext, UseAppCameraContext} from "../../contexts/AppContextExporter.tsx";
import AppStateApiService from "../../services/apis/AppStateApiService.tsx";
import {BaseModal} from "./BaseModal.tsx";
import {Button, Switch} from "../widgets/HtmlControlWrappers.tsx";
import AppLocalStorage from "../storage/AppLocalStorage.ts";
import type {ScannedCoinDto, ScoutMemberCompleteDto} from "../../types/ServerTypes.ts";

interface EditScannedCoinPointsModalProps {
  showModal: boolean;
  setShowModal: React.Dispatch<React.SetStateAction<boolean>>
  scannedCoinDto: ScannedCoinDto;
  setScannedCoinDto: React.Dispatch<React.SetStateAction<ScannedCoinDto>>
}

export default function EditScannedCoinPointsModal({showModal, setShowModal, scannedCoinDto, setScannedCoinDto}: EditScannedCoinPointsModalProps) {
  const [newPointsValue, setNewPointsValue] = useState<number>(0);

  useEffect(() => {
    if (scannedCoinDto) {
      setNewPointsValue(scannedCoinDto.points)
    }
  }, [scannedCoinDto])

  function changeCoinValue() {
    alert('make update')
    setScannedCoinDto(null)
    setShowModal(false)
  }

  return (
    <BaseModal id={"edit-scanned-coin-points-modal"} title={"Change the points for this coin"} show={showModal} onClose={() => {
      setShowModal(false)
    }}>
      {scannedCoinDto && (
        <>
          <div className="row mb-3">
            <div className="col">
              What number of points should this <strong>{scannedCoinDto.coinActivityBase}</strong> coin now have?
            </div>
          </div>
          <div className="row">
            <div className="col text-end">
              <input className="form-control" type="number"
                     value={newPointsValue}
                     onChange={(e) => setNewPointsValue(Number(e.target.value))}
                     min={1}/>
            </div>
          </div>
          <div className="row">
            <div className="col text-end">
              <Button className="btn btn-success" onClick={changeCoinValue}>Change</Button>
            </div>
          </div>
        </>
      )}
    </BaseModal>
  )
}