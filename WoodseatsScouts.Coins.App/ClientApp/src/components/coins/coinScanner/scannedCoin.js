import React from "react";

const ScannedCoin = ({index, coin}) => {
    return <>
     <span key={index} className="tag label label-info">
            <span>{coin.points}</span>
            <span id="remove-score">
                <i data-bind="click: function() { $parent.confirmRemovePoint($data) } " className="remove">&#10060;</i>
            </span>
        </span>
    </>
}

export  default ScannedCoin;