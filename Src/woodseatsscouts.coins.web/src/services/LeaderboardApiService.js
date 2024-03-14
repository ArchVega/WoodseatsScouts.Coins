import axios from "axios";
import {logApi} from "../components/logging/Logger";
import {toast} from "react-toastify";
import Uris from "./Uris";

function LeaderboardApiService() {
    return {
        async getLeaderboardData() {
            const uri = Uris.leaderboard;
            logApi(uri)
            return await axios.get(uri).catch(reason => toast(reason));
        }
    }
}

export default LeaderboardApiService