import React, { useState } from 'react';
import { register, login } from '../../services/authService';
import './SignUp.css';

const SignUp = ({ onRegisterSuccess, onNavigateToLogin }) => {
    const [formData, setFormData] = useState({
        username: '',
        password: '',
        firstName: '',
        lastName: '',
        age: ''
    });
    const [errors, setErrors] = useState({});
    const [apiError, setApiError] = useState('');
    const [successMessage, setSuccessMessage] = useState('');
    const [isLoading, setIsLoading] = useState(false);

    const nameRegex = /^[a-zA-Zа-яА-Я-]+$/;

    const handleChange = (e) => {
        const { id, value } = e.target;
        setFormData((prev) => ({ ...prev, [id]: value }));
    };

    const validateForm = () => {
        const currentErrors = {};

        if (!formData.username.trim()) {
            currentErrors.username = 'Username is required.';
        } else if (formData.username.length < 4 || formData.username.length > 20) {
            currentErrors.username = 'Username must be between 4 and 20 characters.';
        }

        if (!formData.password) {
            currentErrors.password = 'Password is required.';
        } else if (formData.password.length < 5 || formData.password.length > 50) {
            currentErrors.password = 'Password must be between 5 and 50 characters.';
        }

        if (!formData.firstName.trim()) {
            currentErrors.firstName = 'First name is required!';
        } else if (formData.firstName.length < 2) {
            currentErrors.firstName = 'First name must be at least 2 characters!';
        } else if (formData.firstName.length > 30) {
            currentErrors.firstName = 'First name must not exceed 30 characters!';
        } else if (!nameRegex.test(formData.firstName)) {
            currentErrors.firstName = 'First name can contain letters and hyphens!';
        }

        if (!formData.lastName.trim()) {
            currentErrors.lastName = 'Last name is required!';
        } else if (formData.lastName.length < 2) {
            currentErrors.lastName = 'Last name must be at least 2 characters!';
        } else if (formData.lastName.length > 30) {
            currentErrors.lastName = 'Last name must not exceed 30 characters!';
        } else if (!nameRegex.test(formData.lastName)) {
            currentErrors.lastName = 'Last name can contain letters and hyphens!';
        }

        if (formData.age !== '') {
            const ageNum = Number(formData.age);
            if (isNaN(ageNum) || ageNum < 1 || ageNum > 120) {
                currentErrors.age = 'Age must be between 1 and 120!';
            }
        }

        setErrors(currentErrors);
        return Object.keys(currentErrors).length === 0;
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setApiError('');
        setSuccessMessage('');

        if (!validateForm()) {
            return;
        }

        setIsLoading(true);
        try {
            const payload = {
                ...formData,
                age: formData.age !== '' ? Number(formData.age) : null
            };

            await register(payload);

            setSuccessMessage('User created successfully!');

            setTimeout(async () => {
                try {
                    const authData = await login(formData.username, formData.password);
                    if (authData && authData.token) {
                        localStorage.setItem('token', authData.token);
                        if (onRegisterSuccess) {
                            onRegisterSuccess(authData.token);
                        }
                    }
                } catch (authErr) {
                    setApiError('Account created, but automatic login failed. Please log in manually.');
                    setIsLoading(false);
                    onNavigateToLogin();
                }
            }, 2000);

        } catch (error) {
            setApiError(error.message || 'Registration failed.');
            setIsLoading(false);
        }
    };

    return (
        <div className="login-container">
            <div className="login-card signup-card">
                <div className="login-header">
                    <h2>Create Account</h2>
                </div>

                {apiError && <div className="alert alert-danger">{apiError}</div>}
                {successMessage && <div className="alert alert-success">{successMessage}</div>}

                <form onSubmit={handleSubmit} noValidate>
                    <div className="form-row">
                        <div className="form-group">
                            <label htmlFor="firstName">First Name</label>
                            <input
                                type="text"
                                id="firstName"
                                className={`form-control ${errors.firstName ? 'is-invalid' : ''}`}
                                value={formData.firstName}
                                onChange={handleChange}
                                disabled={isLoading}
                                placeholder="John"
                            />
                            {errors.firstName && <div className="error-text">{errors.firstName}</div>}
                        </div>

                        <div className="form-group">
                            <label htmlFor="lastName">Last Name</label>
                            <input
                                type="text"
                                id="lastName"
                                className={`form-control ${errors.lastName ? 'is-invalid' : ''}`}
                                value={formData.lastName}
                                onChange={handleChange}
                                disabled={isLoading}
                                placeholder="Doe"
                            />
                            {errors.lastName && <div className="error-text">{errors.lastName}</div>}
                        </div>
                    </div>

                    <div className="form-row">
                        <div className="form-group username-field">
                            <label htmlFor="username">Username</label>
                            <input
                                type="text"
                                id="username"
                                className={`form-control ${errors.username ? 'is-invalid' : ''}`}
                                value={formData.username}
                                onChange={handleChange}
                                disabled={isLoading}
                                placeholder="Choose username"
                            />
                            {errors.username && <div className="error-text">{errors.username}</div>}
                        </div>

                        <div className="form-group age-field">
                            <label htmlFor="age">Age (Optional)</label>
                            <input
                                type="number"
                                id="age"
                                className={`form-control ${errors.age ? 'is-invalid' : ''}`}
                                value={formData.age}
                                onChange={handleChange}
                                disabled={isLoading}
                                placeholder="25"
                            />
                            {errors.age && <div className="error-text">{errors.age}</div>}
                        </div>
                    </div>

                    <div className="form-group">
                        <label htmlFor="password">Password</label>
                        <input
                            type="password"
                            id="password"
                            className={`form-control ${errors.password ? 'is-invalid' : ''}`}
                            value={formData.password}
                            onChange={handleChange}
                            disabled={isLoading}
                            placeholder="Create password"
                        />
                        {errors.password && <div className="error-text">{errors.password}</div>}
                    </div>

                    <button type="submit" className="btn-login" disabled={isLoading}>
                        {isLoading && !successMessage ? 'Registering...' : successMessage ? 'Logging in...' : 'Sign Up'}
                    </button>
                </form>

                <div className="auth-switch">
                    Already have an account? <span onClick={onNavigateToLogin}>Log In</span>
                </div>
            </div>
        </div>
    );
};

export default SignUp;