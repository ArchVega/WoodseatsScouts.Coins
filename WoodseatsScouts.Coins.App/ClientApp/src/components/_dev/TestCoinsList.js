const TestUsersList = ({onSelected}) => {
    const testUsers = [
        { points: 20 },
        { points: 10 }
    ]
    return <>
        <ul className="list-group list-unstyled">
            {testUsers.map((testCoin, index) => (
                <li key={index} className="list-group-item" style={{cursor: "pointer"}} onClick={() => onSelected(testCoin)}>
                        {testCoin.points}
                </li>                
            ))}
        </ul>
    </>
}

export default TestUsersList;