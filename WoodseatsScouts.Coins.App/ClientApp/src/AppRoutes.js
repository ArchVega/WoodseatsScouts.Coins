import { Counter } from "./components/Counter";
import { CoinsPage } from "./components/coins/coinsPage";
import UsersPage from "./components/users/users";
import DashboardPage from "./components/dashboard/dashboardPage";

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
    path: '/dashboard',
    element: <DashboardPage></DashboardPage>
  },
  {
    path: '/users',
    element: <UsersPage></UsersPage>
  }
];

export default AppRoutes;
