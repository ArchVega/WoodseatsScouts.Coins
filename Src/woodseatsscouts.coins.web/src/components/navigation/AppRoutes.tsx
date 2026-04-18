import MemberDetailsPage from "../../pages/member-details-page/MemberDetailsPage.tsx";
import CoinsPage from "../../pages/home-page/CoinsPage.tsx";
import MembersListPage from "../../pages/members-list-page/MembersListPage.tsx";

const AppRoutes = [
    {
        index: true,
        element: <CoinsPage/>
    },
    {
        path: '/members',
        element: <MembersListPage/>
    },
    //     path: '/leaderboard/members',
    //     element: <MemberLeaderboardPage2026/>
    // },
    {
        path: '/member-details/:memberCode',
        element: <MemberDetailsPage/>,
        hasParams: true
    }
];

export default AppRoutes;