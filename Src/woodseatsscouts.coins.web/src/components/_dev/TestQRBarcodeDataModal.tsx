import {useEffect, useState} from "react";
import QRScanCodeType from "../io/qr-input-devices/QRScanCodeType.ts";
import TestDataApiService from "./TestDataApiService.tsx";
import {BaseModal} from "../modals/BaseModal.tsx";
import type {ScoutMemberCompleteDto} from "../../types/ServerTypes.ts";

type TestQRBarcodeDataModalModel = {
  name: string
  code: string
  selectedCount: number
}

export default function TestQRBarcodeDataModal({testUsersModal, qrScanCodeType, setTestUsersModal, onSelected}) {
  const [testData, setTestData] = useState<TestQRBarcodeDataModalModel[]>([])
  const toggleTestUsersModal = () => setTestUsersModal(!testUsersModal);

  useEffect(() => {
      switch (qrScanCodeType) {
        case QRScanCodeType.Member: {
          async function fetchData() {
            return await TestDataApiService().getMembers()
          }

          fetchData().then(dtos => {
            const testData: TestQRBarcodeDataModalModel[] = []

            dtos.forEach(dto => {
              testData.push({
                name: dto.fullName,
                code: dto.scoutMemberCode,
                selectedCount: 0
              })
            });
            setTestData(testData)
          })
          return
        }
        case QRScanCodeType.Coin: {
          async function fetchData() {
            return await TestDataApiService().getUnscavengedCoins()
          }

          fetchData().then(dtos => {
            const testData: TestQRBarcodeDataModalModel[] = []

            dtos.forEach(dto => {
              testData.push({
                name: `${dto.value} points`,
                code: dto.code,
                selectedCount: 0
              })
            });
            setTestData(testData)
          })
          return
        }
        default:
          throw `No handler for ${qrScanCodeType}`
      }
    }, []
  )

  function renderRow(item: TestQRBarcodeDataModalModel, index: number) {
    const bgClass = item.selectedCount > 0 ? "bg-warning" : "bg-primary"

    return (
      <tr key={index} style={{cursor: "pointer"}}
          onClick={() => {
            item.selectedCount++;
            onSelected(item.code)
          }}>
        <td>{item.name}</td>
        <td>{item.code}</td>
        <td className="text-center">
          <span className={`badge rounded-pill ${bgClass}`}>{item.selectedCount}
          </span>
        </td>
      </tr>
    )
  }

  return (
    <>
      <BaseModal id={"users-test-data-modal"} title={"Test Data"} show={testUsersModal} onClose={() => {
        setTestUsersModal(false)
      }}>
        <div className="row mb-3">
          <div><strong>Select&nbsp;</strong><strong>{qrScanCodeType}</strong></div>
        </div>
        <div className="row">
          <div className="col">
            <table className="table table-bordered table-hover">
              <tbody>
              {testData && testData.map((item, index) => (
                renderRow(item, index)
              ))}
              </tbody>
            </table>
          </div>
        </div>
      </BaseModal>
    </>
  )
}