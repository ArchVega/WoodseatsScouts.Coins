import {Col, Modal, ModalBody, ModalHeader, Row} from "reactstrap";
import QRScanCodeType from "../qrscanners/QRScanCodeType";
import TestData from "./TestData";
import {useEffect, useState} from "react";

const TestQRBarcodeDataModal = ({testUsersModal, qrScanCodeType, setTestUsersModal, onSelected}) => {
    const [testData, setTestData] = useState([])
    const toggleTestUsersModal = () => setTestUsersModal(!testUsersModal);

    useEffect(() => {
        switch (qrScanCodeType) {
            case QRScanCodeType.Member: {
                setTestData([
                    {name: "Crimson Charcoal", code: "M001A000", selectedCount: 0},
                    {name: "Glaucous Jet", code: "M002B000", selectedCount: 0},
                    {name: "Saffron Hunter", code: "M003C000", selectedCount: 0}
                ])
                return
            }
            case QRScanCodeType.Coin: {
                async function fetchData() {
                    return await TestData().getUnscavengedCoins()
                }

                fetchData().then(data => {
                    data.forEach(x => x.selectedCount = 0);
                    setTestData(data)
                })
                return
            }
            default:
                throw `No handler for ${qrScanCodeType}`
        }
    }, []);

    function renderRow(item, index) {
        let data = {}
        switch (qrScanCodeType) {
            case QRScanCodeType.Member: {
                data.name = item.name
                data.value = item.code
                break
            }
            case QRScanCodeType.Coin: {
                data.name = `${item.value} points`
                data.value = item.code
                break
            }
            default:
                throw `No handler for ${qrScanCodeType}`
        }

        const bgClass = item.selectedCount > 0 ? "bg-warning" : "bg-primary"

        return (
            <tr key={index} style={{cursor: "pointer"}}
                onClick={() => {
                    item.selectedCount++;
                    onSelected(item.code)
                }}>
                <td>{data.name}</td>
                <td>{data.value}</td>
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