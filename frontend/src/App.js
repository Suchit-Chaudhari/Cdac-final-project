import React from 'react';
import { BrowserRouter as Router, Route, Routes, Link } from 'react-router-dom';
import './App.css';
import Register from './components/Register';
import Login from './components/Login';
import Navbar from './components/Navbar';
import HomePage from './components/Home';
import About from './pages/About';
import ContactUs from './pages/Contact';
import JobsPage from './pages/JobsPage';

function App() {
  return (
    <Router>
      <div className="App">
        
          <Navbar />
        
          <Routes>
            <Route path="/about" element={<About />} />
            <Route path="/contact" element={<ContactUs />} />
            <Route path="/jobs" element={<JobsPage />} />
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            <Route path="/" element={<HomePage />} />
          </Routes>
      </div>
    </Router>
  );
}



export default App;
