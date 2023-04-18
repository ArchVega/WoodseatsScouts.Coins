import React from "react";
import DataTableComponent, {Styles} from "../common/DataTableComponent";

const MembersRankingPage = () => {
    const columns = React.useMemo(
        () => [
            {
                Header: 'Name',
                columns: [
                    {
                        Header: 'First Name',
                        accessor: 'firstName',
                    },
                    {
                        Header: 'Last Name',
                        accessor: 'lastName',
                    },
                ],
            },
            {
                Header: 'Info',
                columns: [
                    {
                        Header: 'Age',
                        accessor: 'age',
                    },
                    {
                        Header: 'Visits',
                        accessor: 'visits',
                    },
                    {
                        Header: 'Status',
                        accessor: 'status',
                    },
                    {
                        Header: 'Profile Progress',
                        accessor: 'progress',
                    },
                ],
            },
        ],
        []
    )

    function makeData() {
        return [
            {
                firstName: "Boom",
                lastName: "Shakalaka",
                age: 100,
                visits: 3,
                progress: 93,
                status: 'relationship is complicated'
            }
        ]
        
    }

    const data = React.useMemo(() => makeData(2000), [])
    return <>
        <h3>Member rankings</h3>
        <Styles>
            <DataTableComponent columns={columns} data={data}/>
        </Styles>
    </>
}

export default MembersRankingPage;