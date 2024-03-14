import axios from "axios";
import {logApi} from "../components/logging/Logger";
import {toast} from "react-toastify";

function LeaderboardApiService() {
    return {
        async getLeaderboardData() {
            const uri = "http://localhost:7167/home/Report";
            logApi(uri)
            return await axios.get(uri).catch(reason => toast(reason));
        }
    }
}

export default LeaderboardApiService