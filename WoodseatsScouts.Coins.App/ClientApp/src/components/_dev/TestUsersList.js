const TestUsersList = ({onSelected}) => {
    const testUsers = [
        { name: "Scout A", code: "M013B004" },
        { name: "Scout B", code: "M013B005" },
        { name: "Scout C", code: "M013B008" },
    ]
    return <>
        <ul className="list-group list-unstyled">
            {testUsers.map(testUser => (
                <li className="list-group-item" style={{cursor: "pointer"}} onClick={() => onSelected(testUser.code)}>
                        {testUser.name + " - " + testUser.code}
                </li>                
            ))}
        </ul>
    </>
}

export default TestUsersList;