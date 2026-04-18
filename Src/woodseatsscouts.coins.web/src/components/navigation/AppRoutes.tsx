import MemberDetailsPage from "../../pages/member-details-page/MemberDetailsPage.tsx";

const AppRoutes = [
    // {
    //     index: true,
    //     element: <CoinsPage/>
    // },
    // {
    //     path: '/members-old',
    //     element: <MembersListPage/>
    // },
    // {
    //     path: '/members',
    //     element: <MembersListPage2026/>
    // },
    // {
    //     path: '/leaderboard/groups',
    //     element: <LeaderboardPage/>
    // },
    // {
    //     path: '/leaderboard/members',
    //     element: <MemberLeaderboardPage2026/>
    // },
    // {
    //     path: '/leaderboard/members-old',
    //     element: <MemberLeaderboardPage/>
    // },
    {
        path: '/member-details/:memberCode',
        element: <MemberDetailsPage/>,
        hasParams: true
    }
];

export default AppRoutes;