import React, { useState } from 'react';
import Sidebar from './Sidebar';
import TopBar from './TopBar';
import Settings from '../Pages/Settings';
import Users from '../Pages/Users';
import Cars from '../Pages/Cars';
import './Dashboard.css';

const Dashboard = () => {
    const [activePage, setActivePage] = useState('cars');

    const renderPageContent = () => {
        switch (activePage) {
            case 'cars':
                return <Cars />;
            case 'users':
                return <Users />;
            case 'settings':
                return <Settings />;
            default:
                return <div className="page-container"><h1>Welcome</h1></div>;
        }
    };

    return (
        <div className="dashboard-wrapper">
            <TopBar />
            <Sidebar activePage={activePage} setActivePage={setActivePage} />
            <main className="main-content">
                {renderPageContent()}
            </main>
        </div>
    );
};

export default Dashboard;