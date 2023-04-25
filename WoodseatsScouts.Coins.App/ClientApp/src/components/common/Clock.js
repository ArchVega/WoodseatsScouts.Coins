import {useEffect, useState} from "react";

const Clock = () => {
    const [currentTime, setCurrentTime] = useState([])
    
    useEffect(() => {
        let isMounted = true;
        
        const now = new Date();
        setCurrentTime(now.toLocaleString('en-GB'))
        setInterval(() => {
            if (isMounted) {
                const now = new Date();
                setCurrentTime(now.toLocaleString('en-GB'))   
            }            
        }, 1000)
        return () => { isMounted = false; }
    }, [])
    return <>
        {currentTime}
    </>
}

export default Clock