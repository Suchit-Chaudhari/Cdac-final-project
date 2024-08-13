import React, { useEffect, useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css'; // Import Bootstrap CSS
import '../style/Navbar.css';  // Import the CSS file for additional styling
import { Link } from 'react-router-dom';

const Navbar = () => {
    const [profilePic, setProfilePic] = useState(null);
    const [isDropdownOpen, setIsDropdownOpen] = useState(false);
    const user = JSON.parse(localStorage.getItem('user'));
    const role = user?.role;

    const updateProfilePic = async () => {
        if (user && user.userId) {
            try {
                const response = await fetch(`https://localhost:7210/api/users/${user.userId}/profile-picture`);

                if (response.ok) {
                    const data = await response.json();
                    let profilePictureUrl = data.profilePictureUrl;

                    // Normalize the path
                    profilePictureUrl = profilePictureUrl.replace(/\\/g, '/');
                    profilePictureUrl = profilePictureUrl.replace(/^\/uploads\/profile_pictures\/uploads\/profile_pictures\//, '/uploads/profile_pictures/');
                    
                    const fullUrl = `https://localhost:7210${profilePictureUrl}`;

                    console.log("Fetched Profile Picture URL:", fullUrl); // Log the full URL
                    setProfilePic(fullUrl);
                } else {
                    console.error('Failed to fetch profile picture:', response.statusText);
                }
            } catch (error) {
                console.error('Error fetching profile picture:', error);
            }
        } else {
            setProfilePic('/default-profile.png'); // Default profile picture
        }
    };

    useEffect(() => {
        updateProfilePic();

        window.addEventListener('storage', updateProfilePic);

        return () => {
            window.removeEventListener('storage', updateProfilePic);
        };
    }, [user]);

    const handleLogout = () => {
        localStorage.removeItem('user');
        window.location.reload();
    };

    const handleOutsideClick = (event) => {
        if (!event.target.closest('.dropdown-menu') && !event.target.closest('.profile-pic')) {
            setIsDropdownOpen(false);
        }
    };

    useEffect(() => {
        if (isDropdownOpen) {
            window.addEventListener('click', handleOutsideClick);
        } else {
            window.removeEventListener('click', handleOutsideClick);
        }

        return () => {
            window.removeEventListener('click', handleOutsideClick);
        };
    }, [isDropdownOpen]);

    return (
        <nav className="navbar navbar-expand-lg navbar-light bg-light">
            <Link to="/" className="navbar-brand">
                <img src="/images/file.png" alt="Logo" className="navbar-logo" /> {/* Replace with your logo's path */}
            </Link>
            <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                <span className="navbar-toggler-icon"></span>
            </button>
            <div className="collapse navbar-collapse" id="navbarNav">
                <ul className="navbar-nav mr-auto">
                    <li className="nav-item"><Link to="/" className="nav-link">Home</Link></li>
                    <li className="nav-item"><Link to="/about" className="nav-link">About</Link></li>
                    <li className="nav-item"><Link to="/contact" className="nav-link">Contact</Link></li>
                    <li className="nav-item"><Link to="/jobs" className="nav-link">Jobs</Link></li>
                    {role === 'Employer' ? (
                        <li className="nav-item"><Link to="/create-job" className="nav-link">Create Job</Link></li>
                    ) : null}
                </ul>
                <ul className="navbar-nav profile">
                    {user ? (
                        <li className="nav-item dropdown">
                            <img 
                                src={profilePic} 
                                alt="Profile" 
                                className="profile-pic dropdown-toggle" 
                                id="profileDropdown" 
                                role="button" 
                                onClick={() => setIsDropdownOpen(!isDropdownOpen)} 
                                aria-haspopup="true" 
                                aria-expanded={isDropdownOpen ? "true" : "false"} 
                            />
                            {isDropdownOpen && (
                                <div className="dropdown-menu dropdown-menu-right show" aria-labelledby="profileDropdown">
                                    <Link to="/upload-profile-picture" className="dropdown-item" onClick={() => setIsDropdownOpen(false)}>View Profile</Link>
                                    <button className="dropdown-item" onClick={handleLogout}>Logout</button>
                                </div>
                            )}
                        </li>
                    ) : (
                        <>
                            <li className="nav-item"><Link to="/register" className="nav-link">Register</Link></li>
                            <li className="nav-item"><Link to="/login" className="nav-link">Login</Link></li>
                        </>
                    )}
                </ul>
            </div>
        </nav>
    );
};

export default Navbar;
