import {AppContext} from "./contexts/AppContext";
import {Route, Routes} from "react-router-dom";
import AppRoutes from "./components/navigation/AppRoutes.tsx";
import Layout from "./components/common/Layout.tsx";

function App() {
  return (
      <AppContext>
        <Layout>
          <Routes>
            {AppRoutes.map(
                (route, index) => {
                  const {element, ...rest} = route;
                  return <Route key={index} {...rest} element={element}/>;
                })
            }
          </Routes>
        </Layout>
      </AppContext>
  )
}

export default App
