import React, { useEffect, useState } from 'react';
import { api } from '../api';
import SideBar from './sideBar';

// Demo styles, see 'Styles' section below for some notes on use.
import 'react-accessible-accordion/dist/fancy-example.css';

const BadgeContainer: React.FC = () => {
    return (
        <div>
            <SideBar />
            <div>
                <main role="main" className="pb-3">
                    <div className="container-fluid">

                        <div className="row">
                            <main role="main" className="col-md-9 ml-sm-auto col-lg-10 px-4">
                                <div className="container">

                                    <table>
                                        <thead>
                                            <tr>
                                                <th>Badge</th>
                                                <th>Achievement</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td>Golden Crown</td>
                                                <td>Used tube instead of car for a month</td>
                                            </tr>
                                            <tr>
                                                <td>Tortoise</td>
                                                <td>Used tunnel near Isle of Dogs to cross Thames</td>
                                            </tr>
                                            <tr>
                                                <td>Locked</td>
                                                <td>Used all bridge in London at least once.</td>
                                            </tr>
                                            <tr>
                                                <td>Locked</td>
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
