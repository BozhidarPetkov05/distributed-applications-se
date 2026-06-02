import React, { useState, useEffect } from 'react';
import Login from './components/Login/Login';
import SignUp from './components/SignUp/SignUp';
import Dashboard from './components/Layout/Dashboard';

function App() {
    const [currentScreen, setCurrentScreen] = useState('login');

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token) {
            setCurrentScreen('dashboard');
        }
    }, []);

    const handleAuthSuccess = (token) => {
        setCurrentScreen('dashboard');
    };

    if (currentScreen === 'dashboard') {
        return <Dashboard />;
    }

    return (
        <>
            {currentScreen === 'login' ? (
                <Login
                    onLoginSuccess={handleAuthSuccess}
                    onNavigateToSignUp={() => setCurrentScreen('signup')}
                />
            ) : (
                <SignUp
                    onRegisterSuccess={handleAuthSuccess}
                    onNavigateToLogin={() => setCurrentScreen('login')}
                />
            )}
        </>
    );
}

export default App;