import {Col, Row} from "reactstrap";
import {useEffect, useState} from "react";
import Clock from "../../components/clocks/Clock";
import CountdownClock from "../../components/clocks/Countdown";
import LeaderboardApiService from "../../services/LeaderboardApiService";
import Uris from "../../services/Uris";

const LeaderboardPage = () => {
    const [reportData, setReportData] = useState([])
    const [deadlineMilliseconds, setDeadlineMilliseconds] = useState(0)

    useEffect(() => {
        const fetchData = async () => {
            return await LeaderboardApiService().getLeaderboardData();
        }

        fetchData().then(response => setReportData(response.data));
    }, [])

    useEffect(() => {
        const t = +(Date.now() + (reportData.secondsUntilDeadline * 1000)).toFixed(0);
        setDeadlineMilliseconds(t);

        if (!(reportData instanceof Array)) {
            setTimeout(() => {
                if (window.location.pathname === "/leaderboard") {
                    window.location.reload()
                }
            }, reportData.reportRefreshSeconds * 1000)
        }
    }, [reportData])

    return <div id="report-page">
        <Row>
            <Col className="col-6">
                <h1 style={{fontSize: "3vw"}}>{reportData.title}</h1>
            </Col>
            <Col className="col-6 mt-2">
                <Row>
                    <Col className="col-5 white-background text-center" style={{height: "fit-content"}}>
                        <Clock/>
                    </Col>
                    <Col className="col-5 offset-1 text-center countdown-container" style={{fontSize: '1.3em'}}>
                        <CountdownClock deadlineMilliseconds={deadlineMilliseconds}/>
                    </Col>
                </Row>
            </Col>
        </Row>
        <Row style={{height: "1em"}}></Row>
        <Row>
            <Col className="col-6" data-testid="div-last-3-scanned-members">
                <h3>LAST THREE TO SCAN POINTS</h3>
                <Row className="mb-5">
                    {reportData.lastThreeUsersToScanPoints !== undefined && reportData.lastThreeUsersToScanPoints.map((x, index) => (
                        <Col key={index} className="col-4" data-testid="latest-3-scanned-user" data-userid={x.id}>
                            <Row>
                                <Col>
                                    <div className="rect-img-container">
                                        <img
                                            className="rect-img"
                                            src={x.hasImage
                                              ? Uris.memberPhoto(x.id)
                                              : "/images/unknown-member-image.png"}
                                            style={{maxWidth: "100%"}}/>
                                    </div>
                                </Col>
                            </Row>
                            <Row><Col>
                                <span data-testid="latest-3-scanned-user-name" className="bold">{x.firstName + " " + x.lastName}</span>
                            </Col></Row>
                            <Row><Col>
                                <span data-testid="latest-3-scanned-troop-name">{x.troopName}</span>
                            </Col></Row>
                            <Row><Col>
                                <span data-testid="latest-3-scanned-section-name">{x.sectionName}</span>
                            </Col></Row>
                            <Row><Col>
                                <div data-testid="latest-3-scanned-user-points" className="white-background text-center">
                                    <strong>{x.totalPoints}</strong>
                                </div>
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
                        <table className="table report-grid" data-testid="table-top-3-groups">
                            <tbody className="top-3">
                            {reportData.topThreeGroupsInLastHour !== undefined && reportData.topThreeGroupsInLastHour.map((x, index) => (
                                <tr key={index} data-testid="last-hour-top-group">
                                    <td><span className="leader-group-number">{index + 1}</span></td>
                                    <td>
                                        <div data-testid="last-hour-top-group-name">{x.name}</div>
                                    </td>
                                    <td>
                                        <div data-testid="last-hour-top-group-average-points">{x.averagePoints.toFixed(2)}</div>
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
                <table className="table report-grid" data-testid="table-group-leaderboard">
                    <tbody className="all">
                    {reportData.groupsWithMostPointsThisWeekend !== undefined && reportData.groupsWithMostPointsThisWeekend.map((x, index) => (
                        <tr key={index} data-testid="group-leaderboard-group">
                            <td>
                                <div data-testid="group-leaderboard-group-name" className="font-extra-bold">{x.name}</div>
                            </td>
                            <td>
                                <div data-testid="group-leaderboard-group-average-points">{x.averagePoints.toFixed(2)}</div>
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

export default LeaderboardPage