import {logApi} from "../logging/Logger";
import axios from "axios";

function TestData() {
    return {
        async getUnscavengedCoins() {
            const uri = "http://localhost:7167/Sut/Coins"
            logApi(uri)
            const response = await axios.get(uri);

            return response.data.filter(x => !x.isAlreadyScavenged)
        }
    }
}

export default TestData