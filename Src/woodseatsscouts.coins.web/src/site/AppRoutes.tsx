import MemberDetailsPage from "../pages/member-details-page/MemberDetailsPage.tsx";
import CoinsPage from "../pages/coins-page/CoinsPage.tsx";
import MembersListPage from "../pages/members-list-page/MembersListPage.tsx";
import MembersLatestScansPage from "../pages/members-latest-scans-page/MembersLatestScansPage.tsx";
import AdminPage from "../pages/admin-page/AdminPage.tsx";

const AppRoutes = [
    {
        index: true,
        element: <CoinsPage/>
    },
    {
        path: '/member/:memberCode',
        element: <MemberDetailsPage/>,
        hasParams: true
    },
    {
        path: '/members',
        element: <MembersListPage/>
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