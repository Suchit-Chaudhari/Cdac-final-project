import React from 'react';
import '../style/Contact.css';
// For styling

const ContactUs = () => {
  return (
    <div className="contact-us-page">
      <div className="contact-us-container">
        <h1>Contact Us</h1>
        <div className="contact-details">
          <p><strong>Address:</strong> 123 CareerConnect Ave, Mumbai, India</p>
          <p><strong>Phone:</strong> +91 98765 43210</p>
          <p><strong>Email:</strong> contact@careerconnect.com</p>
        </div>
        
        <div className="social-media">
          <h2>Follow Us</h2>
          <p>
            <a href="https://facebook.com/CareerConnect" target="_blank" rel="noopener noreferrer">Facebook</a> | 
            <a href="https://twitter.com/CareerConnect" target="_blank" rel="noopener noreferrer">Twitter</a> | 
            <a href="https://linkedin.com/company/CareerConnect" target="_blank" rel="noopener noreferrer">LinkedIn</a>
          </p>
        </div>
        
        <div className="map">
          <h2>Our Location</h2>
          <iframe 
            title="CareerConnect Location"
            src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3770.9751102374037!2d72.83325377522358!3d19.056068681721224!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x3be7ce6e4b4e0b4d%3A0x2592e8f1a2b1f9b!2sMumbai%2C%20Maharashtra%2C%20India!5e0!3m2!1sen!2sus!4v1691758684321!5m2!1sen!2sus" 
            allowFullScreen="" 
            loading="lazy">
          </iframe>
        </div>
      </div>
    </div>
  );
};

export default ContactUs;