﻿import {useEffect, useState} from "react";
import Countdown, {zeroPad} from "react-countdown";

const CountdownComponent = ({deadlineMilliseconds}) => {
     return  (isNaN(deadlineMilliseconds)) ? <></> : <Countdown
        date={deadlineMilliseconds}
        renderer={({days, hours, minutes, seconds}) => (
            <div className="countdown">
                <span className="hours font-extra-bold-italic">{zeroPad(hours + (days * 24))}</span>
                :
                <span className="minutes font-extra-bold-italic">{zeroPad(minutes)}</span>
                :
                <span className="seconds font-semi-bold-italic">{zeroPad(seconds)}</span>
            </div>
        )}
    />
}

export default CountdownComponent