import {useEffect, useState} from "react";
import AppStateApiService from "../../services/apis/AppStateApiService.tsx";
import getAppSettings from "../../AppSettings.ts";

export default function AdminPage() {
  const [appSettings] = useState(getAppSettings());

  const mode =  import.meta.env.DEV ? "development" : "production";

  useEffect(() => {
  }, []);

  return (
    <>
      <div style={{"display":"flex","justifyContent":"center","alignItems":"center", height:"20vh"}}>
        <strong className={"fs-3"} style={{}}>Running in {mode} mode</strong>
      </div>
      <div style={{"display":"flex","justifyContent":"center","alignItems":"center", height:"20vh"}}>
        <div><pre style={{border: "1px solid grey", padding: "40px"}}>{JSON.stringify(appSettings, null, 2)}</pre></div>
      </div>
    </>
  )
}