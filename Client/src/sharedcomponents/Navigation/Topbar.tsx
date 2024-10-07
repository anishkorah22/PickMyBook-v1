import { Box, Menu, MenuItem, IconButton, Typography } from '@mui/material';
import React, { useState } from 'react';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import { useNavigate } from 'react-router-dom';
import { useUserContext } from '../../context/UserContext';

const Topbar: React.FC = () => {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const navigate = useNavigate();
  const {setUserId,email, roles } = useUserContext(); // Access user details


  // Open the menu
  const handleMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  // Close the menu
  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  // Logout: clear the localStorage and redirect to home
  const handleLogout = () => {
    localStorage.clear(); // Clear all stored data
    setAnchorEl(null);
    setUserId(null);
    navigate('/'); // Redirect to the home page
  };

  return (
    <Box
      sx={{
        height: '4vh',
        width: '100%',
        position: 'fixed',
        top: '0%',
        left: '0',
        zIndex: 1000,
        color: 'white',
        background: '#D0977F',
        padding: '20px',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        boxShadow: '0px 0px 10px rgba(0,0,0,0.2)',
      }}
    >
      {/* Insert the logo image */}
      <img
        src={'src/assets/logo.png'} // Adjust the path accordingly
        alt="Logo"
        style={{ height: '40px', objectFit: 'contain' }} // Adjust the height and style as needed
      />

      {/* IconButton for the AccountCircleIcon */}
      <IconButton
        onClick={handleMenuOpen}
        sx={{ translate: '20em', color: '#000' }}
      >
        <AccountCircleIcon fontSize="large" />
      </IconButton>

      <Menu anchorEl={anchorEl} open={Boolean(anchorEl)} onClose={handleMenuClose}>
        <Box sx={{ margin: 3 }}>
          <Typography>{email ? email : 'Guest'}</Typography>
          {roles && roles.length > 0 ? (
            <Typography>Role: {Array.isArray(roles) ? roles.join(', ') : roles}</Typography>
          ) : (
            <Typography>No roles assigned</Typography>
          )}
          <MenuItem onClick={handleLogout}>Logout</MenuItem>
        </Box>
      </Menu>
    </Box>
  );
};

export default Topbar;
