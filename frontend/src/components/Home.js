import React from 'react';
import { Link } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';

const HomePage = () => {
  const styles = {
    hero: {
      background: 'url(/images/13.jpg) no-repeat center center',
      backgroundSize: 'cover',
      color: 'white',
      width: '100%',
      maxWidth: '1500px',
      margin: 'auto',
      border: '3px solid white',
      padding: '150px 0',
      fontWeight: 'bold',
      marginBottom: '10px',
      textAlign: 'center',
      position: 'relative',
    },
    heroTitle: {
      fontSize: '5rem',
      textShadow: '2px 2px 4px rgba(0, 0, 0, 0.5)',
    },
    heroSubtitle: {
      fontSize: '1.4rem',
      textShadow: '1px 1px 3px rgba(0, 0, 0, 0.5)',
    },
    featureSection: {
      padding: '50px 0',
      background: '#f0f2f5',
    },
    featureContainer: {
      display: 'flex',
      justifyContent: 'space-around',
      alignItems: 'center',
      flexWrap: 'wrap',
      textAlign: 'center',
    },
    featureItem: {
      width: '30%',
      minWidth: '300px',
      margin: '20px',
      background: 'white',
      padding: '20px',
      borderRadius: '12px',
      boxShadow: '0 0 15px rgba(0, 0, 0, 0.1)',
      position: 'relative',
      overflow: 'hidden',
    },
    featureImage: {
      width: '100%',
      height: '150px',           // Adjust height to ensure images fit well
      objectFit: 'cover',        // Ensures images cover the container without distortion
      borderRadius: '8px',
      marginBottom: '15px',
    },
    featureTitle: {
      fontSize: '1.8rem',
      color: '#007bff',
      marginBottom: '10px',
    },
    featureText: {
      fontSize: '1rem',
      color: '#555',
    },
    footer: {
      marginTop: '20px',
      backgroundColor: '#333',
      color: 'white',
      textAlign: 'center',
      padding: '20px 0',
    },
    footerText: {
      margin: 0,
    },
  };

  return (
    <div>
      <div style={styles.hero}>
        <h1 style={styles.heroTitle}>Welcome to CareerConnect</h1>
        <p style={styles.heroSubtitle}>Your gateway to a brighter future. Find your dream job today!</p>
        <Link to="/Jobs/JobList" className="btn btn-primary btn-lg" style={{ color: 'white', fontSize: '1.2rem', textDecoration: 'none', border: '2px solid white', padding: '10px 20px', borderRadius: '5px' }}>Browse Jobs</Link>
      </div>

      <div style={styles.featureSection}>
        <div style={styles.featureContainer}>
          <div style={styles.featureItem}>
            <img src="/images/Search_job_icon.jpg" alt="Search Jobs" style={styles.featureImage} />
            <h2 style={styles.featureTitle}>Search Jobs</h2>
            <p style={styles.featureText}>Find job opportunities that match your skills and preferences. Use our advanced search filters to narrow down your choices.</p>
          </div>
          <div style={styles.featureItem}>
            <img src="/images/resume.jpg" alt="Submit Resume" style={styles.featureImage} />
            <h2 style={styles.featureTitle}>Submit Resume</h2>
            <p style={styles.featureText}>Upload your resume and let employers find you. Get discovered by top companies looking for your expertise.</p>
          </div>
          <div style={styles.featureItem}>
            <img src="/images/download.jpeg" alt="Apply Now" style={styles.featureImage} />
            <h2 style={styles.featureTitle}>Apply Now</h2>
            <p style={styles.featureText}>Apply for jobs with a single click. Track your application status and stay updated on new opportunities.</p>
          </div>
        </div>
      </div>

      <footer style={styles.footer}>
        <div className="container">
          <p style={styles.footerText}>&copy; 2024 CareerConnect. All rights reserved.</p>
        </div>
      </footer>
    </div>
  );
};

export default HomePage;