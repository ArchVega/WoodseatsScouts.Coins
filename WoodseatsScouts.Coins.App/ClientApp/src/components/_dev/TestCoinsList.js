const TestUsersList = ({onSelected}) => {
    const testUsers = [
        { code: "B00320", points: 20 },
        { code: "B00110", points: 10 }
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