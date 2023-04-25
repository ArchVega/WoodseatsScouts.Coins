import UsersPage from "../users/UsersPage";
import React from "react";
import CoinsPage1 from "../coins/coinsPage1";
import ReportPage from "../reports/ReportPage";

const AppRoutes = [
  {
    index: true,
    element: <CoinsPage1 />
  },
  {
    path: '/users',
    element: <UsersPage></UsersPage>
  },
  {
    path: '/report-page',
    element: <ReportPage></ReportPage>
  }
];

export default AppRoutes;
