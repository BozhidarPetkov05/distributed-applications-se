import React from 'react';
import { getAdminStatus } from '../../utils/jwtHelper';

const Sidebar = ({ activePage, setActivePage }) => {
    const isAdmin = getAdminStatus();

    const menuItems = [
        { id: 'cars', label: 'Cars', icon: 'fa-car' },
        { id: 'users', label: 'Users', icon: 'fa-users', adminOnly: true },
        { id: 'settings', label: 'Settings', icon: 'fa-gear' },
    ];

    const handleLogout = () => {
        localStorage.removeItem('token');
        window.location.reload();
    };

    return (
        <aside className="sidebar">
            <div className="sidebar-menu">
                {menuItems.map((item) => {
                    if (item.adminOnly && !isAdmin) return null;

                    return (
                        <div
                            key={item.id}
                            className={`menu-item ${activePage === item.id ? 'active' : ''}`}
                            onClick={() => setActivePage(item.id)}
                        >
                            <i className={`fa-solid ${item.icon}`}></i>
                            <span>{item.label}</span>
                        </div>
                    );
                })}
            </div>

                <div className="sidebar-footer" onClick={handleLogout}>
                <span>Log out</span>
                <i className="fa-solid fa-sign-out-alt"></i>
            </div>
        </aside>
    );
};

export default Sidebar;