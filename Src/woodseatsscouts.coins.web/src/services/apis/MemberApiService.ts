import axios from "axios";
// import {logApi} from "../components/logging/Logger";
import Uris from "./Uris.ts";

function MemberApiService() {
    return {
        async fetchMember(memberQrCode) {
            const uri = Uris.memberByCode(memberQrCode);
            // logApi(uri)
            return await axios.get(uri);
        },

        async fetchMemberWithPoints(memberQrCode) {
            const uri = Uris.memberWithPoints(memberQrCode);
            return await axios.get(uri);
        },

        async fetchMembers() {
            const uri = Uris.member
            return await axios.get(uri);
        }
    }
}

export default MemberApiService