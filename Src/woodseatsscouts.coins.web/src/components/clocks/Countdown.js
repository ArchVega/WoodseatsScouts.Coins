import {useEffect, useState} from "react";
import Countdown, {zeroPad} from "react-countdown";

const CountdownClock = ({deadlineMilliseconds}) => {
     return  (isNaN(deadlineMilliseconds)) ? <></> : <Countdown
        date={deadlineMilliseconds}
        renderer={({days, hours, minutes, seconds}) => (
            <div className="countdown" data-testid="div-countdown-clock">
                <span className="hours font-extra-bold-italic">{zeroPad(hours + (days * 24))}</span>
                :
                <span className="minutes font-extra-bold-italic">{zeroPad(minutes)}</span>
                :
                <span className="seconds font-semi-bold-italic">{zeroPad(seconds)}</span>
            </div>
        )}
    />
}

export default CountdownClock