import axios from "axios";
import {logApi} from "../components/logging/Logger";

function MemberApiService() {
    return {
        async fetchMember(memberQrCode) {
            const uri = `http://localhost:7167/home/GetMemberInfoFromCode?code=${memberQrCode}`;
            logApi(uri)
            return await axios.get(uri);
        },

        async fetchMembers() {
            const uri = "http://localhost:7167/home/GetMembersWithPoints"
            return await axios.get(uri);
        }
    }
}

export default MemberApiService