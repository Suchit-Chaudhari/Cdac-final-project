import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import '../style/About.css';
import TeamMemberCard from '../components/TeamMemberCard';

const teamMembers = [
    {
        name: 'Suchit Ram Chaudhari',
        role: 'Team Lead',
        image: '/images/sch.jpg',
        description: 'Suchit is the driving force behind CareerConnect. With his visionary leadership, he guides the team with unwavering passion and dedication.',
    },
    {
        name: 'Bhargavi Meshram',
        role: 'Team Member',
        image: '/images/bm.jpg',
        description: 'Bhargavi is the tech guru at CareerConnect. She ensures that our platform is cutting-edge and user-friendly, driving innovation and efficiency.',
    },
    {
        name: 'Sourabh Patil',
        role: 'Team Member',
        image: '/images/sp.jpg',
        description: 'Sourabh leads our community engagement efforts, ensuring meaningful connections with our stakeholders and fostering a supportive environment.',
    },
    {
        name: 'Nikhil Mangelkar',
        role: 'Team Member',
        image: '/images/nm.jpg',
        description: 'Nikhil manages our HR department, ensuring that we attract and retain top talent. He is dedicated to maintaining a positive and productive work environment.',
    },
    {
        name: 'Ujjwal Dhake',
        role: 'Team Member',
        image: '/images/ud.jpg',
        description: 'Ujjwal leads our customer support team. He ensures that our users receive the best possible support and that their concerns are addressed promptly.',
    },
];

const About = () => {
    return (
        <div className="container team-section">
            <h1 className="text-center mb-5">Meet Our Team</h1>
            <div className="team-row">
                {teamMembers.map((member, index) => (
                    <TeamMemberCard
                        key={index}
                        name={member.name}
                        role={member.role}
                        image={member.image}
                        description={member.description}
                    />
                ))}
            </div>
        </div>
    );
};

export default About;
