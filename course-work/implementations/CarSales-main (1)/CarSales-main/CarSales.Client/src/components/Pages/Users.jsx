import React, { useState, useEffect } from 'react';
import { getLoggedUserId } from '../../utils/jwtHelper';
import './Users.css';

const Users = () => {
    const [usersData, setUsersData] = useState({
        items: [],
        totalCount: 0,
        page: 1,
        pageSize: 8,
        totalPages: 1,
        hasNext: false,
        hasPrevious: false
    });

    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const [searchUsername, setSearchUsername] = useState('');
    const [filterAdmin, setFilterAdmin] = useState('');
    const [isDescending, setIsDescending] = useState(true);

    const [currentPage, setCurrentPage] = useState(1);
    const [pageSize, setPageSize] = useState(8);

    const [activeFilters, setActiveFilters] = useState({
        username: '',
        isAdmin: '',
        isDescending: true
    });

    const [selectedUser, setSelectedUser] = useState(null);
    const [modalActionLoading, setModalActionLoading] = useState(false);
    const [modalError, setModalError] = useState(null);

    const currentUserId = getLoggedUserId();

    const fetchUsers = async (pageToFetch, filtersToUse) => {
        setLoading(true);
        setError(null);

        try {
            const token = localStorage.getItem('token');

            const queryParams = new URLSearchParams();

            queryParams.append('page', pageToFetch);
            queryParams.append('pageSize', pageSize);
            queryParams.append('isDescending', filtersToUse.isDescending);

            if (filtersToUse.username.trim()) {
                queryParams.append('username', filtersToUse.username.trim());
            }

            if (filtersToUse.isAdmin !== '') {
                queryParams.append('isAdmin', filtersToUse.isAdmin);
            }

            const response = await fetch(
                `https://localhost:7125/api/users?${queryParams.toString()}`,
                {
                    method: 'GET',
                    headers: {
                        Authorization: `Bearer ${token}`,
                        'Content-Type': 'application/json'
                    }
                }
            );

            if (response.status === 401 || response.status === 403) {
                throw new Error('Access denied. Admin rights required.');
            }

            if (!response.ok) {
                throw new Error('Failed to fetch users database.');
            }

            const data = await response.json();

            setUsersData({
                items: data.items || [],
                totalCount: data.totalCount || 0,
                page: data.page || 1,
                pageSize: data.pageSize || pageSize,
                totalPages: data.totalPages || 1,
                hasNext: data.hasNext || false,
                hasPrevious: data.hasPrevious || false
            });
        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchUsers(currentPage, activeFilters);
    }, [currentPage, pageSize]);

    const handleApplyFilters = (e) => {
        e.preventDefault();

        const newFilters = {
            username: searchUsername,
            isAdmin: filterAdmin,
            isDescending: isDescending
        };

        setActiveFilters(newFilters);
        setCurrentPage(1);

        fetchUsers(1, newFilters);
    };

    const handlePageSizeChange = (e) => {
        const newSize = Number(e.target.value);

        setPageSize(newSize);
        setCurrentPage(1);

        fetchUsers(1, activeFilters);
    };

    const handleDeleteUser = async (userId) => {
        if (
            !window.confirm(
                `Are you absolutely sure you want to permanently delete this user?`
            )
        ) {
            return;
        }

        setModalActionLoading(true);
        setModalError(null);

        try {
            const token = localStorage.getItem('token');

            const response = await fetch(
                `https://localhost:7125/api/users/${userId}`,
                {
                    method: 'DELETE',
                    headers: {
                        Authorization: `Bearer ${token}`,
                        'Content-Type': 'application/json'
                    }
                }
            );

            if (!response.ok) {
                throw new Error(
                    'Failed to delete the user. Please try again.'
                );
            }

            setSelectedUser(null);

            fetchUsers(currentPage, activeFilters);
        } catch (err) {
            setModalError(err.message);
        } finally {
            setModalActionLoading(false);
        }
    };

    const handleToggleAdminStatus = async (user) => {
        setModalActionLoading(true);
        setModalError(null);

        const updatedAdminStatus = !user.isAdmin;

        const putRequestBody = {
            username: user.username,
            password: user.password || '',
            firstName: user.firstName,
            lastName: user.lastName,
            age: user.age !== undefined ? user.age : null,
            isAdmin: updatedAdminStatus
        };

        try {
            const token = localStorage.getItem('token');

            const response = await fetch(
                `https://localhost:7125/api/users/${user.id}`,
                {
                    method: 'PUT',
                    headers: {
                        Authorization: `Bearer ${token}`,
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(putRequestBody)
                }
            );

            if (!response.ok) {
                throw new Error(
                    'Failed to update user status on the server.'
                );
            }

            setSelectedUser((prev) => ({
                ...prev,
                isAdmin: updatedAdminStatus
            }));

            fetchUsers(currentPage, activeFilters);
        } catch (err) {
            setModalError(err.message);
        } finally {
            setModalActionLoading(false);
        }
    };

    if (error) {
        return (
            <div className="users-status-container error-box">
                <i className="fa-solid fa-triangle-exclamation"></i>
                {' '}Error: {error}
            </div>
        );
    }

    return (
        <div className="users-container">
            <h1 className="users-main-title">User Management Panel</h1>

            {/* FILTERS */}
            <form className="users-filter-bar" onSubmit={handleApplyFilters}>
                <div className="filter-group search-input">
                    <label>Search Username</label>

                    <div className="input-icon-wrapper">
                        <i className="fa-solid fa-magnifying-glass"></i>

                        <input
                            type="text"
                            placeholder="Type username..."
                            value={searchUsername}
                            onChange={(e) =>
                                setSearchUsername(e.target.value)
                            }
                        />
                    </div>
                </div>

                <div className="filter-group">
                    <label>Role Filter</label>

                    <select
                        value={filterAdmin}
                        onChange={(e) => setFilterAdmin(e.target.value)}
                    >
                        <option value="">All Roles</option>
                        <option value="true">Administrators Only</option>
                        <option value="false">Regular Users</option>
                    </select>
                </div>

                <div className="filter-group">
                    <label>Order by Creation</label>

                    <select
                        value={isDescending}
                        onChange={(e) =>
                            setIsDescending(e.target.value === 'true')
                        }
                    >
                        <option value="true">Newest First</option>
                        <option value="false">Oldest First</option>
                    </select>
                </div>

                <button type="submit" className="btn-filter-search">
                    <i className="fa-solid fa-filter"></i>
                    {' '}Apply Filters
                </button>
            </form>

            {/* TABLE */}
            <div className="users-table-wrapper">
                {loading ? (
                    <div className="users-table-loading">
                        Loading system accounts...
                    </div>
                ) : usersData.items.length === 0 ? (
                    <div className="users-empty-box">
                        <i className="fa-solid fa-users-slash"></i>

                        <p>
                            No users found matching the filter criteria.
                        </p>
                    </div>
                ) : (
                    <table className="users-table">
                        <thead>
                            <tr>
                                <th>Username</th>
                                <th>Full Name</th>
                                <th>Role</th>
                                <th>Member Since</th>
                            </tr>
                        </thead>

                        <tbody>
                            {usersData.items.map((u) => (
                                <tr
                                    key={u.id}
                                    className="clickable-row"
                                    onClick={() => {
                                        setSelectedUser(u);
                                        setModalError(null);
                                    }}
                                    title="Click to view full details"
                                >
                                    <td className="cell-username">
                                        {u.username}
                                    </td>

                                    <td>
                                        {u.firstName} {u.lastName}
                                    </td>

                                    <td>
                                        {u.isAdmin ? (
                                            <span className="badge-role admin">
                                                Admin
                                            </span>
                                        ) : (
                                            <span className="badge-role user">
                                                User
                                            </span>
                                        )}
                                    </td>

                                    <td>
                                        {new Date(
                                            u.createdAt
                                        ).toLocaleDateString('en-GB')}
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                )}
            </div>

            {/* PAGINATION */}
            <div className="pagination-footer">
                <span className="pagination-info">
                    Total: <strong>{usersData.totalCount}</strong> users |
                    {' '}Page <strong>{usersData.page}</strong> of{' '}
                    <strong>{usersData.totalPages}</strong>
                </span>

                <div className="pagination-page-size">
                    <label>Users per page:</label>

                    <select
                        value={pageSize}
                        onChange={handlePageSizeChange}
                    >
                        <option value={4}>4</option>
                        <option value={8}>8</option>
                        <option value={12}>12</option>
                        <option value={16}>16</option>
                        <option value={20}>20</option>
                    </select>
                </div>

                <div className="pagination-buttons">
                    <button
                        className="btn-page"
                        disabled={!usersData.hasPrevious || loading}
                        onClick={() =>
                            setCurrentPage((prev) => prev - 1)
                        }
                    >
                        <i className="fa-solid fa-chevron-left"></i>
                        {' '}Previous
                    </button>

                    <button
                        className="btn-page"
                        disabled={!usersData.hasNext || loading}
                        onClick={() =>
                            setCurrentPage((prev) => prev + 1)
                        }
                    >
                        Next{' '}
                        <i className="fa-solid fa-chevron-right"></i>
                    </button>
                </div>
            </div>

            {/* MODAL */}
            {selectedUser && (
                <div
                    className="modal-overlay"
                    onClick={() => setSelectedUser(null)}
                >
                    <div
                        className="modal-content-box"
                        onClick={(e) => e.stopPropagation()}
                    >
                        <div className="modal-header">
                            <h2>User Detailed Information</h2>

                            <button
                                className="modal-close-x"
                                onClick={() => setSelectedUser(null)}
                            >
                                &times;
                            </button>
                        </div>

                        {modalError && (
                            <div className="modal-status-error">
                                <i className="fa-solid fa-circle-exclamation"></i>
                                {' '}{modalError}
                            </div>
                        )}

                        <div className="user-details-modal-grid">
                            <div className="detail-field">
                                <label>Database ID (GUID)</label>

                                <p className="mono-text">
                                    {selectedUser.id}
                                </p>
                            </div>

                            <div className="detail-field">
                                <label>Username</label>

                                <p className="highlight-text">
                                    {selectedUser.username}
                                </p>
                            </div>

                            <div className="detail-field">
                                <label>First Name</label>

                                <p>{selectedUser.firstName}</p>
                            </div>

                            <div className="detail-field">
                                <label>Last Name</label>

                                <p>{selectedUser.lastName}</p>
                            </div>

                            <div className="detail-field">
                                <label>Account Role</label>

                                <p>
                                    {selectedUser.isAdmin ? (
                                        <span className="badge-role admin">
                                            Administrator
                                        </span>
                                    ) : (
                                        <span className="badge-role user">
                                            Regular User
                                        </span>
                                    )}
                                </p>
                            </div>

                            <div className="detail-field">
                                <label>
                                    Registration Date (CreatedAt)
                                </label>

                                <p>
                                    {new Date(
                                        selectedUser.createdAt
                                    ).toLocaleString('en-GB')} Local
                                </p>
                            </div>

                            <div className="detail-field full-width">
                                <label>
                                    Last Internal Modification
                                    (LastChanged)
                                </label>

                                <p className="cell-muted">
                                    {new Date(
                                        selectedUser.lastChanged
                                    ).toLocaleString('en-GB', {
                                        timeZone: 'UTC'
                                    })}{' '}
                                    UTC
                                </p>
                            </div>
                        </div>

                        {/* ACTIONS */}
                        <div className="modal-user-actions">
                            {/* Скриване на бутона за роли, ако избраният потребител е текущо логнатият */}
                            {selectedUser.id !== currentUserId && (
                                <button
                                    className="btn-modal-action-admin"
                                    onClick={() =>
                                        handleToggleAdminStatus(selectedUser)
                                    }
                                    disabled={modalActionLoading}
                                >
                                    <i className="fa-solid fa-user-shield"></i>
                                    {selectedUser.isAdmin
                                        ? 'Demote to Regular User'
                                        : 'Promote to Admin'}
                                </button>
                            )}

                            <button
                                className="btn-modal-action-delete"
                                onClick={() =>
                                    handleDeleteUser(selectedUser.id)
                                }
                                disabled={modalActionLoading}
                            >
                                {modalActionLoading ? (
                                    'Processing...'
                                ) : (
                                    <>
                                        <i className="fa-solid fa-user-minus"></i>
                                        {' '}Delete Account
                                    </>
                                )}
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default Users;