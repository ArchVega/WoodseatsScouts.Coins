import MemberDetailsPage from "../pages/member-details-page/MemberDetailsPage.tsx";
import CoinsPage from "../pages/coins-page/CoinsPage.tsx";
import ScoutMembersListPage from "../pages/members-list-page/ScoutMembersListPage.tsx";
import MembersLatestScansPage from "../pages/members-latest-scans-page/MembersLatestScansPage.tsx";
import AdminPage from "../pages/admin-page/AdminPage.tsx";

const AppRoutes = [
    {
        index: true,
        element: <CoinsPage/>
    },
    {
        path: '/members/:memberId',
        element: <MemberDetailsPage/>,
        hasParams: true
    },
    {
        path: '/members',
        element: <ScoutMembersListPage/>
    },
    {
        path: '/members/latest-scans',
        element: <MembersLatestScansPage/>
    },
    {
        path: '/admin',
        element: <AdminPage/>
    }
];

export default AppRoutes;