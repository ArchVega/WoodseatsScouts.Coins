const TestUsersList = ({onSelected}) => {
    const testUsers = [
        { name: "Conner", code: "M999B001" },
        { name: "Orlando", code: "M999B002" },
        { name: "Calvin", code: "M999B003" },
        { name: "Dillon", code: "M074C004" },
        { name: "Josiah", code: "M074C005" }
    ]
    return <>
        <ul className="list-group list-unstyled">
            {testUsers.map((testUser, index) => (
                <li key={index} className="list-group-item" style={{cursor: "pointer"}} onClick={() => onSelected(testUser.code)}>
                        {testUser.name + " - " + testUser.code}
                </li>                
            ))}
        </ul>
    </>
}

export default TestUsersList;