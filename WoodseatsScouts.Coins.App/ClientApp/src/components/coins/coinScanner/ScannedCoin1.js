const ScannedCoin1 = ({coin, removeCoin}) => {
    return <>
     <span className="tag label label-info">
            <span>{coin.points}</span>
            <span id="remove-score">
                <i onClick={() => removeCoin(coin)} className="remove">&#10060;</i>
            </span>
        </span>
    </>
}

export  default ScannedCoin1;