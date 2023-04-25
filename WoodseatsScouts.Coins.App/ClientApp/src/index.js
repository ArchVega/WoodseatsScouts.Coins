import React from "react";
import { render } from "react-dom";
import App from "./App";
import { BrowserRouter } from 'react-router-dom'
import './css/bootstrap.min.css'
import './css/site.scss';

const root = document.getElementById("root");

render(
    <BrowserRouter>
        <App />
    </BrowserRouter>,
    root
)
