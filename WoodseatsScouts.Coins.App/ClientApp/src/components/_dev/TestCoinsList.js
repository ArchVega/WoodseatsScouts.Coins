const TestUsersList = ({onSelected}) => {
    const testCoins = [
        { code: "B00320", pointValue: 20 },
        { code: "B00110", pointValue: 10 },
        { code: "B00105", pointValue: 5 },
    ]
    return <>
        <ul className="list-group list-unstyled">
            {testCoins.map((testCoin, index) => (
                <li key={index} className="list-group-item" style={{cursor: "pointer"}} onClick={() => onSelected(testCoin)}>
                        {testCoin.pointValue}
                </li>                
            ))}
        </ul>
    </>
}

export default TestUsersList;