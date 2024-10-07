import { Box } from '@mui/material';
import React, { useState } from 'react';
import { Link, useLocation } from 'react-router-dom';
import { useUserContext } from '../../context/UserContext';

const SideBar: React.FC = () => {
  const location = useLocation();
  const {roles} =  useUserContext(); // Access user details

  const [activePath, setActivePath] = useState(location.pathname);
  // Function to handle button click
  const handleLinkClick = (path: string) => {
    setActivePath(path);
  };

  // Check if the user has the "user" role
  const isUserRole = roles ? roles.includes('User') : false;

  return (
    <Box
      sx={{
        height: '100vh',
        width: '17%',
        position: 'fixed',
        top: '0%',
        left: '0',
        background: 'rgba(227,194,182)',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        boxShadow: '0px 0px 10px rgba(0,0,0,0.2)',
      }}
    >
      <span style={{ display: 'flex', flexDirection: 'column' }}>
        <Link to="/dashboard" style={{ textDecoration: 'none', marginBottom: '20px' }}>
          <button
            onClick={() => handleLinkClick('/dashboard')}
            style={{
              width: "12.5em",
              borderRadius: 3,
              cursor: "pointer",
              border: activePath === '/dashboard' ? '1px solid #ffffff' : 'none',
              background: activePath === '/dashboard' ? '#D9A07C' : 'none',
              fontSize: 17,
              padding: '10px 20px',
              color: activePath === '/dashboard' ? '#fff' : '#000',
            }}
          >
            Dashboard
          </button>
        </Link>
        <Link to="/book-catalogue" style={{ textDecoration: 'none', marginBottom: '20px' }}>
          <button
            onClick={() => handleLinkClick('/book-catalogue')}
            style={{
              width: "12.5em",
              borderRadius: 3,
              cursor: "pointer",
              border: activePath === '/book-catalogue' ? '1px solid #ffffff' : 'none',
              background: activePath === '/book-catalogue' ? '#D9A07C' : 'none',
              fontSize: 17,
              padding: '10px 20px',
              color: activePath === '/book-catalogue' ? '#fff' : '#000',
            }}
          >
            Book Catalogue
          </button>
        </Link>
        {/* Conditionally render the Users tab based on roles */}
        {!isUserRole && (
          <Link to="/users" style={{ textDecoration: 'none', marginBottom: '20px' }}>
            <button
              onClick={() => handleLinkClick('/users')}
              style={{
                width: "12.5em",
                borderRadius: 3,
                cursor: "pointer",
                border: activePath === '/users' ? '1px solid #ffffff' : 'none',
                background: activePath === '/users' ? '#D9A07C' : 'none',
                fontSize: 17,
                padding: '10px 20px',
                color: activePath === '/users' ? '#fff' : '#000',
              }}
            >
              Users
            </button>
          </Link>
        )}
      </span>
    </Box>
  );
};

export default SideBar;
