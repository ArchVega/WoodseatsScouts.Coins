import CoinsPage from "../../pages/homepage/CoinsPage";
import MembersListPage from "../../pages/memberslistpage/MembersListPage";
import LeaderboardPage from "../../pages/leaderboardpage/LeaderboardPage";
import VotePage from "../../pages/vote/VotePage";
import VoteResultsPage from "../../pages/vote/VoteResultsPage";
import MemberLeaderboardPage from "../../pages/leaderboardpage/MemberLeaderboardPage";
import MembersListPage2026 from "../../pages/memberslistpage/MembersListPage2026";
import MemberDetailsPage from "../../pages/memberdetailspage/MemberDetailsPage";

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
        path: '/members-old',
        element: <MembersListPage/>
    },
    {
        path: '/members',
        element: <MembersListPage2026/>
    },
    {
        path: '/leaderboard/groups',
        element: <LeaderboardPage/>
    },
    {
        path: '/leaderboard/members',
        element: <MemberLeaderboardPage/>
    },
    {
        path: '/vote-results',
        element: <VoteResultsPage/>
    },
    {
        path: '/member-details/:memberCode',
        element: <MemberDetailsPage/>,
        hasParams: true
    }
];

export default AppRoutes;
