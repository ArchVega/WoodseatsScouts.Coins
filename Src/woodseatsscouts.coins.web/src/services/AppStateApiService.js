import axios from "axios";
import Uris from "./Uris";

const AppStateApiService = () => {
    return {
        getAppSate: (responseFunc) => {
            async function fetch() {
                const response = await axios.get(Uris.appState);
                return response.data
            }

            fetch().then(response => {
                responseFunc(response)
            });
        }
    }
}

export default AppStateApiService;