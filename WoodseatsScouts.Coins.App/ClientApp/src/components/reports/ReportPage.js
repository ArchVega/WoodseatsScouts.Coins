import {Col, Row} from "reactstrap";
import {useEffect, useState} from "react";
import Countdown, {zeroPad} from "react-countdown";
import Clock from "../common/Clock";
import CountdownComponent from "../common/CountdownComponent";
import {toast} from "react-toastify";

const ReportPage = () => {
    const rows = [1, 2, 3, 4, 5, 6, 7, 8, 9]
    const [reportData, setReportData] = useState([])
    const [deadlineMilliseconds, setDeadlineMilliseconds] = useState(0)

    useEffect(() => {
        const fetchUser = async () => {
            const requestOptions = {
                headers: {'Content-Type': 'application/json'}
            };

            const response = await fetch("home/Report", requestOptions);
            if (response.status === 200) {
                const data = await response.json();
                setReportData(data);
            } else {
                const data = await response.text();
                toast(data)
            }
        }

        fetchUser().then();
    }, [])

    useEffect(() => {
        const t = +(Date.now() + (reportData.secondsUntilDeadline * 1000)).toFixed(0);
        setDeadlineMilliseconds(t);
        
        if (!(reportData instanceof Array) ) {            
            setTimeout(() => {
                if (window.location.pathname === "/report-page") {
                    window.location.reload()
                }
            }, reportData.reportRefreshSeconds * 1000)   
        }
    }, [reportData])

    function lookupSectionName(code) {
        switch (code) {
            case "B":
                return "Beavers"
            case "C":
                return "Cubs"
            case "E":
                return "Explorers"
            case "A":
                return "Adults"
            case "S":
                return "Scouts"
        }
    }

    return <div id="report-page">
        <Row>
            <Col className="col-6">
                <h1 style={{fontSize: "3vw"}}>{reportData.title}</h1>
            </Col>
            <Col className="col-6 mt-2">
                <Row>
                    <Col className="col-5 white-background text-center" style={{height: "fit-content"}}>
                        <Clock></Clock>
                    </Col>
                    <Col className="col-5 offset-1 text-center countdown-container" style={{fontSize: '1.3em'}}>
                        <CountdownComponent deadlineMilliseconds={deadlineMilliseconds}></CountdownComponent>
                    </Col>
                </Row>
            </Col>
        </Row>
        <Row style={{height: "3em"}}></Row>

        <Row>
            <Col className="col-6">
                <h3>LAST THREE TO SCAN POINTS</h3>
                <Row className="mb-5">
                    {reportData.lastThreeUsersToScanPoints !== undefined && reportData.lastThreeUsersToScanPoints.map((x, index) => (
                        <Col key={index} className="col-4">
                            <Row><Col><img
                                src={x.hasImage ? `member-images/${x.id}.jpg?x=${new Date().getTime()}` : "images/unknown-member-image.png"}
                                style={{maxWidth: "100%"}}/></Col>
                            </Row>
                            <Row><Col><span className="bold">{x.firstName + " " + x.lastName}</span></Col></Row>
                            <Row><Col>{x.troopName}</Col></Row>
                            <Row><Col>{lookupSectionName(x.section)}</Col></Row>
                            <Row><Col>
                                <div className="white-background text-center"><strong>{x.totalPoints}</strong></div>
                            </Col></Row>
                        </Col>
                    ))}
                    {reportData.lastThreeUsersToScanPoints !== undefined && reportData.lastThreeUsersToScanPoints.length === 0 ? <>
                        <Col className="col-4">
                            <span>None yet...</span>
                        </Col>
                    </> : null}
                </Row>
                <Row>
                    <Col>
                        <Row>
                            <Col>
                                <h3 className="font-extra-bold">Top three groups in the last hour...</h3>
                            </Col>
                        </Row>
                        <table className="table report-grid">
                            <tbody className="top-3">
                            {reportData.topThreeGroupsInLastHour !== undefined && reportData.topThreeGroupsInLastHour.map((x, index) => (
                                <tr key={index}>
                                    <td><span className="leader-group-number">{index + 1}</span></td>
                                    <td>
                                        <div>{x.name}</div>
                                    </td>
                                    <td>                                        
                                        <div>{x.averagePoints.toFixed(2)}</div>
                                    </td>
                                </tr>
                            ))}
                            {reportData.topThreeGroupsInLastHour !== undefined && reportData.topThreeGroupsInLastHour.length === 0 ?
                                <tr>
                                    <td colSpan={3}>None yet...</td>
                                </tr>
                                : null}
                            </tbody>
                        </table>
                    </Col>
                </Row>
            </Col>
            <Col className="col-6">
                <Row>
                    <Col>
                        <h3 className="font-extra-bold">Groups with the most points this weekend...</h3>
                    </Col>
                </Row>
                <table className="table report-grid">
                    <tbody className="all">
                    {reportData.groupsWithMostPointsThisWeekend !== undefined && reportData.groupsWithMostPointsThisWeekend.map((x, index) => (
                        <tr key={index}>
                            <td>
                                <div className="font-extra-bold">{x.name}</div>
                            </td>
                            <td>
                                <div>{x.averagePoints.toFixed(2)}</div>
                            </td>
                        </tr>
                    ))}
                    {reportData.groupsWithMostPointsThisWeekend !== undefined && reportData.groupsWithMostPointsThisWeekend.length === 0 ?
                        <tr>
                            <td colSpan={3}>None yet...</td>
                        </tr>
                        : null}
                    </tbody>
                </table>
            </Col>
        </Row>
    </div>
}

export default ReportPage