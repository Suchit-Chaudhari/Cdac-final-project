import React, { useState } from 'react';
import axios from 'axios';

const JobFilter = ({ onFilter }) => {
  const [filters, setFilters] = useState({
    jobTitle: '',
    location: '',
    jobType: '',
    experienceLevel: '',
    industry: '',
    minSalary: '',
    maxSalary: ''
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFilters({
      ...filters,
      [name]: value
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.get('/api/jobs/Filter', { params: filters });
      onFilter(response.data);
    } catch (error) {
      console.error("Error fetching filtered jobs", error);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <div>
        <label>Job Title:</label>
        <input
          type="text"
          name="jobTitle"
          value={filters.jobTitle}
          onChange={handleChange}
        />
      </div>
      <div>
        <label>Location:</label>
        <input
          type="text"
          name="location"
          value={filters.location}
          onChange={handleChange}
        />
      </div>
      <div>
        <label>Job Type:</label>
        <input
          type="text"
          name="jobType"
          value={filters.jobType}
          onChange={handleChange}
        />
      </div>
      <div>
        <label>Experience Level:</label>
        <input
          type="text"
          name="experienceLevel"
          value={filters.experienceLevel}
          onChange={handleChange}
        />
      </div>
      <div>
        <label>Industry:</label>
        <input
          type="text"
          name="industry"
          value={filters.industry}
          onChange={handleChange}
        />
      </div>
      <div>
        <label>Min Salary:</label>
        <input
          type="number"
          name="minSalary"
          value={filters.minSalary}
          onChange={handleChange}
        />
      </div>
      <div>
        <label>Max Salary:</label>
        <input
          type="number"
          name="maxSalary"
          value={filters.maxSalary}
          onChange={handleChange}
        />
      </div>
      <button type="submit">Filter Jobs</button>
    </form>
  );
};

export default JobFilter;
