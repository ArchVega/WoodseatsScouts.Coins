import { Counter } from "./components/Counter";
import { CoinsPage } from "./components/coins/coinsPage";
import UsersPage from "./components/users/users";
import MemberRankingDashboardPage from "./components/dashboard/memberRankingDashboardPage";
import BasesDashboardPage from "./components/dashboard/basesDashboardPage";

const AppRoutes = [
  {
    index: true,
    element: <CoinsPage />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/member-ranking',
    element: <MemberRankingDashboardPage></MemberRankingDashboardPage>
  },
  {
    path: '/bases',
    element: <BasesDashboardPage></BasesDashboardPage>
  },
  {
    path: '/users',
    element: <UsersPage></UsersPage>
  }
];

export default AppRoutes;
