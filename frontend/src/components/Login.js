import React, { useState } from 'react';
import axios from 'axios';
import '../style/Login.css';

function Login() {
  const [formData, setFormData] = useState({
    email: '',
    password: ''
  });

  const [errorMessage, setErrorMessage] = useState('');

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setErrorMessage('');

    const loginData = {
      email: formData.email,
      password: formData.password
    };

    try {
      const response = await axios.post('https://localhost:7210/api/users/signin', loginData, {
        headers: { 'Content-Type': 'application/json' }
      });
      
      // Store user data in localStorage
      const userData = response.data;
      localStorage.setItem('user', JSON.stringify(userData));
      
      alert('Login successful');
      // Redirect or perform other actions based on the user role
      if (userData.role === 'JobSeeker') {
        // Redirect to job seeker dashboard
        window.location.href = '/jobseeker-dashboard';
      } else if (userData.role === 'Employer') {
        // Redirect to employer dashboard
        window.location.href = '/employer-dashboard';
      } else {
        // Handle other roles or default case
        window.location.href = '/';
      }
    } catch (error) {
      console.error('Error during login:', error.response || error);
      setErrorMessage('Invalid email or password');
    }
  };

  return (
    <div className="login-container">
      <h2>Login</h2>
      <form onSubmit={handleSubmit} className="login-form">
        <div className="form-group">
          <label htmlFor="email">Email address</label>
          <input
            type="email"
            id="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            required
          />
        </div>
        <div className="form-group">
          <label htmlFor="password">Password</label>
          <input
            type="password"
            id="password"
            name="password"
            value={formData.password}
            onChange={handleChange}
            required
          />
        </div>
        {errorMessage && <p className="error-message">{errorMessage}</p>}
        <button type="submit" className="btn btn-primary">Login</button>
      </form>
    </div>
  );
}

export default Login;
