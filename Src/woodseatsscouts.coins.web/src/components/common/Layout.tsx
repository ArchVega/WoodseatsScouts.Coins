import SiteDevBar from "../_dev/siteDevBar.tsx";
import NavMenu from "../navigation/NavMenu.tsx";
// import {ToastContainer} from "react-toastify";
// import 'react-toastify/dist/ReactToastify.css';

export default function Layout({children}) {
  return (
    <div className={"site-container d-flex flex-column"}>
      <SiteDevBar/>
      <NavMenu/>
      <div className="page-container container flex-grow-1">
        {children}
        {/*<ToastContainer />*/}
      </div>
      <footer className="page-footer">
        Scouts Coin App
      </footer>
    </div>
  )
}