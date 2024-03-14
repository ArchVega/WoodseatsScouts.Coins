import axios from "axios";
import {BaseUri} from "./ApiService";

const AppStateApiService = () => {
    return {
        getAppSate: (responseFunc) => {
            async function fetch() {
                const response = await axios.get(`${BaseUri}/AppState`);
                return response.data
            }

            fetch().then(response => {
                responseFunc(response)
            });
        }
    }
}

export default AppStateApiService;