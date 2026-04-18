import MemberDetailsPage from "../pages/member-details-page/MemberDetailsPage.tsx";
import CoinsPage from "../pages/home-page/CoinsPage.tsx";
import MembersListPage from "../pages/members-list-page/MembersListPage.tsx";
import MemberLeaderboardPage from "../pages/member-leaderboard-page/MemberLeaderboardPage2026.tsx";

const AppRoutes = [
    {
        index: true,
        element: <CoinsPage/>
    },
    {
        path: '/members',
        element: <MembersListPage/>
    },
    {
        path: '/leaderboard/members',
        element: <MemberLeaderboardPage/>
    },
    {
        path: '/member-details/:memberCode',
        element: <MemberDetailsPage/>,
        hasParams: true
    }
];

export default AppRoutes;