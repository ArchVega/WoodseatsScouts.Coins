import {useEffect, useRef} from "react";
import './ScannedCoin.scss'

export default function ScannedCoin({coin, removeCoin, isLast}) {
  const scrollRef = useRef(null)

  useEffect(() => {
    const executeScroll = () => scrollRef.current.scrollIntoView()
    executeScroll()
  }, [])

  function getClassName() {
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

    return tagClass
  }

  return <>
     <span className={getClassName()} ref={scrollRef}>
       <div className="remove-points" onClick={() => removeCoin(coin)}>
            x
        </div>
        <div className="coin-value">{coin.pointValue}</div>
    </span>
  </>
}