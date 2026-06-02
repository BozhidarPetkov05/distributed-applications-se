import React, { useState } from 'react';
import { login } from '../../services/authService';
import './Login.css';

const Login = ({ onLoginSuccess, onNavigateToSignUp }) => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [errors, setErrors] = useState({});
    const [apiError, setApiError] = useState('');
    const [isLoading, setIsLoading] = useState(false);

    const validateForm = () => {
        const currentErrors = {};

        if (!username.trim()) {
            currentErrors.username = 'Username is required.';
        } else if (username.length < 4 || username.length > 20) {
            currentErrors.username = 'Username must be between 4 and 20 characters.';
        }

        if (!password) {
            currentErrors.password = 'Password is required.';
        } else if (password.length < 5 || password.length > 50) {
            currentErrors.password = 'Password must be between 5 and 50 characters.';
        }

        setErrors(currentErrors);
        return Object.keys(currentErrors).length === 0;
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setApiError('');

        if (!validateForm()) {
            return;
        }

        setIsLoading(true);
        try {
            const data = await login(username, password);

            if (data && data.token) {
                localStorage.setItem('token', data.token);
                if (onLoginSuccess) {
                    onLoginSuccess(data.token);
                }
            }
        } catch (error) {
            setApiError(error.message || 'Invalid username or password.');
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="login-container">
            <div className="login-card">
                <div className="login-header">
                    <h2>Car Sales Management<br />System</h2>
                </div>

                {apiError && <div className="alert alert-danger">{apiError}</div>}

                <form onSubmit={handleSubmit} noValidate>
                    <div className="form-group">
                        <label htmlFor="username">Username</label>
                        <input
                            type="text"
                            id="username"
                            className={`form-control ${errors.username ? 'is-invalid' : ''}`}
                            value={username}
                            onChange={(e) => setUsername(e.target.value)}
                            disabled={isLoading}
                            placeholder="Enter username"
                        />
                        {errors.username && <div className="error-text">{errors.username}</div>}
                    </div>

                    <div className="form-group">
                        <label htmlFor="password">Password</label>
                        <input
                            type="password"
                            id="password"
                            className={`form-control ${errors.password ? 'is-invalid' : ''}`}
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            disabled={isLoading}
                            placeholder="Enter password"
                        />
                        {errors.password && <div className="error-text">{errors.password}</div>}
                    </div>

                    <button type="submit" className="btn-login" disabled={isLoading}>
                        {isLoading ? 'Processing...' : 'Log In'}
                    </button>
                </form>

                <div className="auth-switch">
                    Don't have an account? <span onClick={onNavigateToSignUp}>Sign Up</span>
                </div>
            </div>
        </div>
    );
};

export default Login;