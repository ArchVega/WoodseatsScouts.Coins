const ScannedCoin = ({coin}) => {
    return <>
     <span className="tag label label-info">
            <span>{coin.points}</span>
            <span id="remove-score">
                <i data-bind="click: function() { $parent.confirmRemovePoint($data) } " className="remove">&#10060;</i>
            </span>
        </span>
    </>
}

export  default ScannedCoin;