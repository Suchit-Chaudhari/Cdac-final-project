import React, { useState } from 'react';
import JobFilter from '../components/JobFilter';


const JobsPage = () => {
  const [jobs, setJobs] = useState([]);

  const handleFilter = (filteredJobs) => {
    setJobs(filteredJobs);
  };

  return (
    <div>
      <h1>Job Listings</h1>
      <JobFilter onFilter={handleFilter} />
      <div>
        {jobs.length > 0 ? (
          jobs.map(job => (
            <div key={job.id}>
              <h2>{job.jobTitle}</h2>
              <p>{job.location}</p>
              <p>{job.jobType}</p>
              <p>{job.experienceLevel}</p>
              <p>{job.industry}</p>
              <p>${job.salary}</p>
            </div>
          ))
        ) : (
          <p>No jobs found</p>
        )}
      </div>
    </div>
  );
};

export default JobsPage;
