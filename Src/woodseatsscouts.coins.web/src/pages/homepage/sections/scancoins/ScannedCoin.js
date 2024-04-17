import {useEffect, useRef} from "react";

function ScannedCoin ({coin, removeCoin, isLast}) {
    const scrollRef = useRef(null)
    
    useEffect(() => {
        const executeScroll = () => scrollRef.current.scrollIntoView()        
        executeScroll()
    }, [])

    let tagClass = "coin-random-odd"

    if (coin !== undefined) {
        if (coin.pointValue === 20) {
            tagClass = "coin-20"
        } else if (coin.pointValue === 10) {
            tagClass = "coin-10"
        }
    }
    
    tagClass = "coin label label-info text-center " + tagClass
    if (isLast) {
        tagClass += " coin-last-added"
    }
    
    return <>
     <span className={tagClass} ref={scrollRef}>
            <div className="coin-value font-extra-bold text-white">{coin.pointValue}</div>
            <span className="remove-score">
                <i onClick={() => removeCoin(coin)} className="remove"><div style={{fontSize: '2em'}} >x</div></i>
            </span>
        </span>
    </>
}

export default ScannedCoin;