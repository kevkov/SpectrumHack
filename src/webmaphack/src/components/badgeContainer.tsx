import React, { useEffect, useState } from 'react';
import { api } from '../api';
import SideBar from './sideBar';

// Demo styles, see 'Styles' section below for some notes on use.
import 'react-accessible-accordion/dist/fancy-example.css';

const BadgeContainer: React.FC = () => {
    return (
        <div>
            <header>
                <nav className="navbar navbar-expand-sm navbar-toggleable-sm navbar-dark bg-dark border-bottom box-shadow mb-3">
                    <div className="container-fluid">
                        <a className="navbar-brand text-white" href="/">Spectrum Hackathon</a>
                        <button className="navbar-toggler" type="button" data-toggle="collapse" data-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                            aria-expanded="false" aria-label="Toggle navigation">
                            <span className="navbar-toggler-icon"></span>
                        </button>
                        <div className="navbar-collapse collapse d-sm-inline-flex flex-sm-row-reverse">
                            <ul className="navbar-nav flex-grow-1">
                                <li className="nav-item">
                                    <a className="nav-link text-light" href="/">Home</a>
                                </li>
                            </ul>
                        </div>
                    </div>
                </nav>
            </header>
            <div>
                <main role="main" className="pb-3">
                    <div className="container-fluid">
                        <div className="row">
                            <SideBar />
                            <main role="main" className="col-md-9 ml-sm-auto col-lg-10 px-4">

                                <div className="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-1 pb-1 mb-3 border-bottom">
                                    <table>
                                        <thead>
                                            <tr>
                                                <th>Badge</th>
                                                <th>Achievement</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td><img className="rounded mx-auto d-block" width="100" height="100" src="goldCrown.jpg" alt="Golden Crown" /></td>
                                                <td>Used tube instead of car for a month</td>
                                            </tr>
                                            <tr>
                                                <td><img className="rounded mx-auto d-block" width="100" height="100" src="normaltortoise.jpg" alt="Tortoise" /></td>
                                                <td>Used tunnel near Isle of Dogs to cross Thames</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img className="rounded mx-auto d-block" width="100" height="100" src="lockedbadge.jpg" alt="Locked" />
                                                </td>
                                                <td>Used all bridge in London at least once.</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img className="rounded mx-auto d-block" width="100" height="100" src="lockedbadge.jpg" alt="Locked" />
                                                </td>
                                                <td>Commute by walking part of your journey for 10 miles.</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </main>
                        </div>
                    </div>
                </main>
            </div>

            <footer className="border-top footer text-muted">
                <div className="container">
                    &copy; 2019 - Spectrum
        </div>
            </footer>
        </div>
    );
}

export default BadgeContainer;
