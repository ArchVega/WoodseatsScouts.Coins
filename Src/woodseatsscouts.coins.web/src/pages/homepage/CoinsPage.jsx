import {Col, Row} from "reactstrap";
import React, {useEffect, useState} from "react";
import SiteSpinner from "../../components/spinner/SiteSpinner";
import MemberApiService from "../../services/MemberApiService";
import {toast} from "react-toastify";
import AudioFx from "../../fx/AudioFx";
import {logError, logReactSet, logDebug, logReactUseEffect, logAttention} from "../../components/logging/Logger";
import ScanMemberSection from "./sections/ScanMemberSection";
import SectionNames from "./sections/SectionNames";
import ScanCoinsSection from "./sections/scancoins/ScanCoinsSection";
import HaulResultsSection from "./sections/HaulSummary";
import {toastError} from "../../components/toaster/toaster";

export function CoinsPage() {
    const [loading, setLoading] = useState(false)
    const [sectionName, setSectionName] = useState(SectionNames.ScanMember)
    const [section, setSection] = useState(null);
    const [memberQrCode, setMemberQrCode] = useState("")
    const [member, setMember] = useState(null)
    const [haulResult, setHaulResult] = useState(null)

    useEffect(() => {
        if (memberQrCode != null && memberQrCode.trim().length > 0) {
            logDebug(`Fetching member data for code ${memberQrCode}`)

            setLoading(true)

            async function fetchData() {
                AudioFx().playMemberScannedAudio()
                return await MemberApiService().fetchMember(memberQrCode)
            }

            fetchData()
                .then(async value => {
                    const data = (await value.data)
                    logReactSet("Setting member", data)
                    setMember(data);
                })
                .catch(async reason => {
                    logError("Setting member", reason)
                    toastError(reason)
                    setMember(null)
                })
                .finally(() => {
                    setLoading(false)
                })
        }

    }, [memberQrCode]);

    useEffect(() => {
        logReactUseEffect("member", member)
        if (member !== null) {
            setSectionName(SectionNames.ScanCoins)
        }
    }, [member]);

    useEffect(() => {
        if (haulResult != null) {
            setSectionName(SectionNames.HaulSummary)
            new AudioFx().playHaulCompleteAudio();
        }
    }, [haulResult]);

    useEffect(() => {
        logReactUseEffect("sectionName", sectionName)
        logAttention(loading)

        if (loading) {
            return setSection(<SiteSpinner/>)
        }

        function getSection() {
            switch (sectionName) {
                case SectionNames.ScanMember: {
                    return (<ScanMemberSection qrCode={memberQrCode} setQrCode={setMemberQrCode}/>)
                }
                case SectionNames.ScanCoins: {
                    return (<ScanCoinsSection member={member} setHaulResult={setHaulResult}/>)
                }
                case SectionNames.HaulSummary: {
                    return (<HaulResultsSection member={member} haulResult={haulResult}/>)
                }
                default: {
                    throw `Handler not defined ${sectionName}`
                }
            }
        }

        setSection(getSection());
    }, [sectionName]);

    return (
        <Row>
            <Col className="mb-2">
                <Row>
                    <Col>
                        {loading
                            ? (<SiteSpinner/>)
                            : (<section>{section}</section>)}
                    </Col>
                </Row>
            </Col>
        </Row>
    )
}

export default CoinsPage
{/*<div>{process.env.REACT_APP_VERSION}</div>*/
}