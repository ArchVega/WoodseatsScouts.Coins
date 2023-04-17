import { CoinsPage } from "../coins/coinsPage";
import UsersPage from "../users/UsersPage";
import NewUserPage from "../users/NewUserPage";
import React from "react";
import MembersRankingPage from "../rankings/membersRankingPage";
import BasesRankingPage from "../rankings/basesRankingPage";

const AppRoutes = [
  {
    index: true,
    element: <CoinsPage />
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
