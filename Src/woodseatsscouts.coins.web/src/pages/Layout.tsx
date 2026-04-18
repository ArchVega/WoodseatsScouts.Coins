import SiteDevBar from "../components/_dev/siteDevBar.tsx";
import NavMenu from "../site/NavMenu.tsx";
import {ToastContainer} from "react-toastify";
import 'react-toastify/dist/ReactToastify.css';

export default function Layout({children}) {
  return (
    <div className="site-container d-flex flex-column">
      <SiteDevBar/>
      <NavMenu/>
      <div className="page-container container">
        {children}
        <ToastContainer/>
      </div>
    </div>
  )
}