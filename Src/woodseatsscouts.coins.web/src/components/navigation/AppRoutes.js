import CoinsPage from "../../pages/homepage/CoinsPage";
import MembersListPage from "../../pages/memberslistpage/MembersListPage";
import LeaderboardPage from "../../pages/leaderboardpage/LeaderboardPage";

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
        path: '/leaderboard',
        element: <LeaderboardPage/>
    }
];

export default AppRoutes;
