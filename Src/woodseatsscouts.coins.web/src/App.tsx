import AppContext from "./contexts/AppContext";
import {Route, Routes} from "react-router-dom";
import AppRoutes from "./site/AppRoutes.tsx";
import Layout from "./pages/Layout.tsx";

export default function App() {
  return (
    <AppContext>
      <Layout>
        <Routes>
          {AppRoutes.map(
            (route: any, index: number) => {
              const {element, ...rest} = route;
              return <Route key={index} {...rest} element={element}/>;
            })
          }
        </Routes>
      </Layout>
    </AppContext>
  )
}