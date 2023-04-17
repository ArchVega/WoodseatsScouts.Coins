import { Counter } from "./components/Counter";
import { CoinsPage } from "./components/coins/coinsPage";
import UsersPage from "./components/users/users";
import NewUserPage from "./components/users/newUserPage";
import React from "react";
import MembersRankingPage from "./components/rankings/membersRankingPage";
import BasesRankingPage from "./components/rankings/basesRankingPage";

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
    path: '/members-ranking',
    element: <MembersRankingPage></MembersRankingPage>
  },
  {
    path: '/bases-ranking',
    element: <BasesRankingPage></BasesRankingPage>
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
