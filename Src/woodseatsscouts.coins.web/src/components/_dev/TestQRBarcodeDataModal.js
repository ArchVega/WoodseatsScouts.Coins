import {Col, Modal, ModalBody, ModalHeader, Row} from "reactstrap";
import QRScanCodeType from "../qrscanners/QRScanCodeType";
import TestDataApiService from "./TestDataApiService";
import {useEffect, useState} from "react";

const TestQRBarcodeDataModal = ({testUsersModal, qrScanCodeType, setTestUsersModal, onSelected}) => {
    const [testData, setTestData] = useState([])
    const toggleTestUsersModal = () => setTestUsersModal(!testUsersModal);

    useEffect(() => {
        switch (qrScanCodeType) {
            case QRScanCodeType.Member: {
                async function fetchData() {
                    return await TestDataApiService().getMembers()
                }

                fetchData().then(data => {
                    data.forEach(x => {
                        x.name = x.fullName
                        x.value = x.memberCode
                        x.selectedCount = 0
                    });
                    setTestData(data)
                })
                return
            }
            case QRScanCodeType.Coin: {
                async function fetchData() {
                    return await TestDataApiService().getUnscavengedCoins()
                }

                fetchData().then(data => {
                    data.forEach(x => {
                        x.name = `${x.value} points`
                        x.value = x.code
                        x.selectedCount = 0
                    });
                    setTestData(data)
                })
                return
            }
            default:
                throw `No handler for ${qrScanCodeType}`
        }
    }, []);

    function renderRow(item, index) {
        const bgClass = item.selectedCount > 0 ? "bg-warning" : "bg-primary"

        return (
            <tr key={index} style={{cursor: "pointer"}}
                onClick={() => {
                    item.selectedCount++;
                    onSelected(item.value)
                }}>
                <td>{item.name}</td>
                <td>{item.value}</td>
                <td className="text-center">
                    <span className={`badge rounded-pill ${bgClass}`}>{item.selectedCount}
                    </span>
                </td>
            </tr>
        )
    }

    return <>
        <Modal isOpen={testUsersModal} toggle={toggleTestUsersModal}>
            <ModalHeader toggle={toggleTestUsersModal}>Test Data</ModalHeader>
            <ModalBody>
                <Row className="mb-3">
                    <Col><strong>Select&nbsp;</strong><strong>{qrScanCodeType}</strong></Col>
                </Row>
                <Row>
                    <Col>
                        <table className="table table-bordered table-hover">
                            <tbody>
                            {testData && testData.map((item, index) => (
                                renderRow(item, index)
                            ))}
                            </tbody>
                        </table>
                    </Col>
                </Row>
            </ModalBody>
        </Modal>
    </>
}

export default TestQRBarcodeDataModal;