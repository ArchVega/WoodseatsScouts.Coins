import axios from "axios";
import {logApi} from "../components/logging/Logger";
import Uris from "./Uris";

function MemberApiService() {
    return {
        async fetchMember(memberQrCode) {
            const uri = Uris.member(memberQrCode);
            logApi(uri)
            return await axios.get(uri);
        },

        async fetchMembers() {
            const uri = Uris.members
            return await axios.get(uri);
        }
    }
}

export default MemberApiService