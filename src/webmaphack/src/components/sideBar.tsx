import React from 'react';
import './sideBar.css';
import mainPageBadge from './mainpagebadge.png';

const SideBar: React.FC = () => {
  return (
    <nav className="col-md-2 d-none d-md-block bg-light">
            <div>
                <div className="container">
                    <div className="row">
                        <img className="rounded mx-auto d-block" width="250" height="300" src="https://upload.wikimedia.org/wikipedia/commons/5/56/Sadiq_Khan_November_2016.jpg" alt="Sadiq Khan" />
                    </div>
                    <div className="row pt-2">
                        <div className="col-7">
                            <h6>Sadiq Khan</h6>
                        </div>
                        <div className="col-5 text-right">
                                <h6>Level 32</h6>
                        </div>
                    </div>
                    <div className="row pt-2">
                      <div className="col-8">
                          <img className="rounded mx-auto d-block" width="40" height="40" src="{mainPageBadge}" alt="badge" />
                        </div>
                        <div className="col-4 text-right">
                            <span className="badge badge-pill badge-success">123 points</span>
                        </div>
                    </div>
                </div>
                <ul className="nav flex-column">
                    <li><hr/></li>
                    <li className="nav-item">
                        <a className="nav-link" href="/">
                            Pay
                        </a>
                    </li>
                    <li className="nav-item">
                        <a className="nav-link" href="/">
                            Rewards
                        </a>
                    </li>
                    <li className="nav-item">
                        <Link className="nav-link" to="/badges">Badges</Link>                        
                    </li>
                    <li className="nav-item">
                        <a className="nav-link" href="/">
                            Places
                        </a>
                    </li>
                    <li className="nav-item">
                        <a className="nav-link" href="/">
                            History
                        </a>
                    </li>
                    <li className="nav-item">
                        <a className="nav-link" href="/">
                            Notifications
                        </a>
                    </li>
                    <li className="nav-item">
                        <a className="nav-link" href="/">
                            Help
                        </a>
                    </li>
                    <li className="nav-item dropdown">
                        <a className="nav-link dropdown-toggle" data-toggle="dropdown" href="/" role="button" aria-haspopup="true" aria-expanded="false">Feedback</a>
                        <div className="dropdown-menu">
                            <a className="dropdown-item" href="/">Contact us</a>
                            <a className="dropdown-item" href="/">Help improve the app</a>
                            <a className="dropdown-item" href="/">Rate app</a>
                            <a className="dropdown-item" href="/">Share app</a>
                        </div>
                    </li>
                    <li className="nav-item dropdown">
                        <a className="nav-link dropdown-toggle" data-toggle="dropdown" href="/" role="button" aria-haspopup="true" aria-expanded="false">Other</a>
                        <div className="dropdown-menu">
                            <a className="dropdown-item" href="/">Privacy Policy</a>
                            <a className="dropdown-item" href="/">Terms of Service</a>
                            <a className="dropdown-item" href="/">Data Sources</a>
                            <a className="dropdown-item" href="/">Acknowledgements</a>
                        </div>
                    </li>
                    <li className="nav-item">
                        <a className="nav-link" href="/">
                            Settings
                        </a>
                    </li>
                    <li className="nav-item">
                        <a className="nav-link" href="/">
                            Logout
                        </a>
                    </li>
                </ul>
            </div>
        </nav>  
  );
}

export default SideBar;
