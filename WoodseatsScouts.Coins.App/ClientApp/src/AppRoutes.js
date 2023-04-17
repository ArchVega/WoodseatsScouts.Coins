import { Counter } from "./components/Counter";
import { CoinsPage } from "./components/coins/coinsPage";
import UsersPage from "./components/users/users";
import MemberRankingDashboardPage from "./components/dashboard/memberRankingDashboardPage";
import BasesDashboardPage from "./components/dashboard/basesDashboardPage";
import NewUserPage from "./components/users/newUserPage";
import React from "react";

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
    path: '/new-user',
    element: <NewUserPage></NewUserPage>
  },
  {
    path: '/users',
    element: <UsersPage></UsersPage>
  }
];

export default AppRoutes;
