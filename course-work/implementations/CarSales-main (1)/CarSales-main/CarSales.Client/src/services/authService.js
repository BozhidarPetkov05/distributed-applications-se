const BASE_URL = 'https://localhost:7125/api';

export const login = async (username, password) => {
    const details = { username, password };
    const formBody = new URLSearchParams(details);

    const response = await fetch(`${BASE_URL}/auth`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded;charset=UTF-8',
        },
        body: formBody,
    });

    if (!response.ok) {
        const errorData = await response.json().catch(() => ({}));
        throw new Error(errorData.message || 'Invalid username or password.');
    }

    return await response.json();
};

export const register = async (userData) => {
    const response = await fetch(`${BASE_URL}/users`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(userData),
    });

    if (!response.ok) {
        const errorData = await response.json().catch(() => ({}));
        throw new Error(errorData.message || 'Registration failed. Please try again.');
    }

    return await response.json().catch(() => ({}));
};