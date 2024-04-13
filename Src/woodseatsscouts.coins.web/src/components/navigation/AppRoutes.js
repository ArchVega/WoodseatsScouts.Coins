import CoinsPage from "../../pages/homepage/CoinsPage";
import MembersListPage from "../../pages/memberslistpage/MembersListPage";
import LeaderboardPage from "../../pages/leaderboardpage/LeaderboardPage";
import VotePage from "../../pages/vote/VotePage";
import VoteResultsPage from "../../pages/vote/VoteResultsPage";

const AppRoutes = [
    {
        index: true,
        element: <CoinsPage/>
    },
    {
        path: '/vote',
        element: <VotePage/>
    },
    {
        path: '/members',
        element: <MembersListPage/>
    },
    {
        path: '/leaderboard',
        element: <LeaderboardPage/>
    },
    {
        path: '/vote-results',
        element: <VoteResultsPage/>
    }
];

export default AppRoutes;
