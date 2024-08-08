import React from 'react';
import { Link } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';

const HomePage = () => {
  const styles = {
    hero: {
      background: 'url(/images/banner2.jpg) no-repeat center center',
      backgroundSize: 'cover',
      color: 'black',
      padding: '100px 0',
      fontWeight: 'bold',
      marginBottom: '20px',
      textAlign: 'center'
    },
    heroTitle: {
      fontSize: '3rem'
    },
    heroSubtitle: {
      fontSize: '1.2rem'
    },
    feature: {
      padding: '30px 0'
    },
    featureTitle: {
      fontSize: '2rem',
      marginBottom: '15px'
    },
    featureText: {
      fontSize: '1rem'
    },
    footer: {
      marginTop: '20px',
      backgroundColor: 'black',
      color: 'white',
      textAlign: 'center',
      padding: '20px 0'
    }
  };

  return (
    <div>
      <div style={styles.hero}>
        <h1 style={styles.heroTitle}>Welcome to CareerConnect</h1>
        <p style={styles.heroSubtitle}>Your gateway to a brighter future. Find your dream job today!</p>
        <Link to="/Jobs/JobList" className="btn btn-primary btn-lg">Browse Jobs</Link>
      </div>

      <div className="container feature" style={styles.feature}>
        <div className="row">
          <div className="col-md-4 text-center">
            <img src="/images/Search_job_icon.jpg" alt="Search Jobs" className="img-fluid" />
            <h2 style={styles.featureTitle}>Search Jobs</h2>
            <p style={styles.featureText}>Find job opportunities that match your skills and preferences. Use our advanced search filters to narrow down your choices.</p>
          </div>
          <div className="col-md-4 text-center">
            <img src="/images/resume.jpg" alt="Submit Resume" className="img-fluid" />
            <h2 style={styles.featureTitle}>Submit Resume</h2>
            <p style={styles.featureText}>Upload your resume and let employers find you. Get discovered by top companies looking for your expertise.</p>
          </div>
          <div className="col-md-4 text-center">
            <img src="/images/download.jpeg" alt="Apply Now" className="img-fluid" />
            <h2 style={styles.featureTitle}>Apply Now</h2>
            <p style={styles.featureText}>Apply for jobs with a single click. Track your application status and stay updated on new opportunities.</p>
          </div>
        </div>
      </div>

      <footer style={styles.footer}>
        <div className="container">
          <p>&copy; 2024 CareerConnect. All rights reserved.</p>
        </div>
      </footer>
    </div>
  );
};

export default HomePage;