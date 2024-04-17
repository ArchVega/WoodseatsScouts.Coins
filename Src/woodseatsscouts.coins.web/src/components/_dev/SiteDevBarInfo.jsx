import {Tooltip} from "bootstrap"
import React from "react";

const SiteDevBarInfo = (appModeContext) => {
    if (appModeContext === undefined) {
        return <></>
    }

    if (appModeContext.appMode === "Development") {
        return <>
            <div className="scouts-sitebar-appmode-development">
                <strong>AppMode:&nbsp;</strong>Development
            </div>
        </>
    }

    if (appModeContext.appMode === "AcceptanceTest") {
        return <>
            <div className="scouts-sitebar-appmode-acceptancetesting">
                <strong>AppMode:&nbsp;</strong>
                Acceptance Testing
            </div>
        </>
    }

    return <>
    </>
}

export default SiteDevBarInfo