import React, { useEffect, useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css'; // Import Bootstrap CSS
import '../style/Navbar.css';  // Import the CSS file for additional styling

const Navbar = () => {
    const [profilePic, setProfilePic] = useState(null);
    const user = JSON.parse(localStorage.getItem('user'));
    const updateProfilePic = () => {
        
        setProfilePic(user && user.profilePictureUrl ? user.profilePictureUrl : null);
    };

    useEffect(() => {
        // Initial load
        updateProfilePic();

        // Listen for localStorage changes
        window.addEventListener('storage', updateProfilePic);

        // Cleanup listener
        return () => {
            window.removeEventListener('storage', updateProfilePic);
        };
    }, []);

    return (
        <nav className="navbar navbar-expand-lg navbar-light bg-light">
            <a href="/" className="navbar-brand">JobPortal</a>
            <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                <span className="navbar-toggler-icon"></span>
            </button>
            <div className="collapse navbar-collapse" id="navbarNav">
                <ul className="navbar-nav mr-auto">
                    <li className="nav-item"><a href="/" className="nav-link">Home</a></li>
                    <li className="nav-item"><a href="/about" className="nav-link">About</a></li>
                    <li className="nav-item"><a href="/contact" className="nav-link">Contact</a></li>
                    <li className="nav-item"><a href="/jobs" className="nav-link">Jobs</a></li>
                </ul>
                <ul className="navbar-nav profile">
                    {user ? (
                        <li className="nav-item">
                            <img src={profilePic} alt="Profile" className="profile-pic" />
                        </li>
                    ) : (
                        <>
                            <li className="nav-item"><a href="/register" className="nav-link">Register</a></li>
                            <li className="nav-item"><a href="/login" className="nav-link">Login</a></li>
                        </>
                    )}
                </ul>
            </div>
        </nav>
    );
};

export default Navbar;
