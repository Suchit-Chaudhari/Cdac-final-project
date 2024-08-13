import React from 'react';

const TeamMemberCard = ({ name, role, image, description }) => (
    <div className="card">
        <img src={image} className="card-img-top" alt={name} />
        <div className="card-body text-center">
            <h5 className="card-title">{name}</h5>
            <p className="card-text1">{role}</p>
            <p className="card-text">{description}</p>
        </div>
    </div>
);

export default TeamMemberCard;
