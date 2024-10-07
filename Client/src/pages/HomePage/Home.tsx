import React, { useState, useEffect } from 'react';
import { Box, Button, Modal, TextField, Typography } from '@mui/material';
import homeImage from '../../assets/home.png'; // Adjust the path based on your folder structure
import logo from '../../assets/logo.png';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { useUserContext } from '../../context/UserContext';

const Home: React.FC = () => {
  const [open, setOpen] = useState(false);
  const [email, setEmail] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();
  const {setUserId} = useUserContext();

  // Check if the user is already logged in on component mount
  useEffect(() => {
    const token = localStorage.getItem('jwtToken');
    const storedUserId = localStorage.getItem('userId');

    // If token and userId exist in localStorage, navigate to the dashboard
    if (token && storedUserId) {
      setUserId(storedUserId);
      navigate('/dashboard');
    }
  }, [navigate, setUserId]);

  // Open modal to input email
  const handleOpen = () => setOpen(true);

  // Close modal
  const handleClose = () => {
    setError('');
    setOpen(false);
  };

  // Handle the login request with the email input
  const handleLogin = async () => {
    try {
      const response = await axios.post(
        'https://localhost:7131/api/Auth/authenticate',
        { email },
        {
          headers: { 'Content-Type': 'application/json' },
        }
      );

      const { token, userId, roles } = response.data; // Assuming the response contains token, userId, and roles

      // Store token, user ID, email, and roles in localStorage
      localStorage.setItem('jwtToken', token);
      localStorage.setItem('userId', userId); 
      localStorage.setItem('email', email);
      localStorage.setItem('roles', JSON.stringify(roles)); // Store roles as a JSON string

      // Update the userId in the App component
      setUserId(userId);
      // Redirect to dashboard or protected route
      navigate('/dashboard');

      // Close the modal after successful login
      handleClose();
    } catch (err) {
      setError('Invalid login credentials.');
      console.error('Error during login:', err);
    }
  };

  // When the user clicks submit, we log them in
  const isValidEmail = (email: string) => {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
  };
  
  const handleEmailSubmit = () => {
    if (email.trim() && isValidEmail(email)) {
      handleLogin();
    } else if (!email.trim()) {
      setError('Email is required.');
    } else {
      setError('Invalid email format.');
    }
  };

  return (
    <Box>
      <img src={homeImage} alt="Home" style={{ width: '100%', height: '96vh' }} />

      {/* Sign in Card */}
      <Box
        bgcolor={'white'}
        sx={{
          display: 'flex',
          width: '30%',
          flexDirection: 'column',
          position: 'absolute',
          zIndex: 1,
          p: 6,
          alignItems: 'center',
          justifyContent: 'center',
          top: '27%',
          left: '33%',
          borderRadius: 3,
        }}
      >
        <img src={logo} alt="logo" style={{ width: 200, borderRadius: 10 }} />
        <h5>Book management made simple.</h5>
        <Button variant="contained" onClick={handleOpen}>
          Sign in with Email
        </Button>
      </Box>

      {/* Modal for email input */}
      <Modal open={open} onClose={handleClose}>
        <Box
          sx={{
            position: 'absolute',
            top: '50%',
            left: '50%',
            transform: 'translate(-50%, -50%)',
            width: 400,
            bgcolor: 'background.paper',
            boxShadow: 24,
            p: 4,
            borderRadius: 2,
          }}
        >
          <Typography variant="h6" component="h2">
            Sign in with Email
          </Typography>

          {/* Email input field */}
          <TextField
            fullWidth
            label="Email Address"
            variant="outlined"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            sx={{ mb: 2 }}
          />

          {/* Display error if invalid login */}
          {error && <Typography color="error" sx={{ mb: 2 }}>{error}</Typography>}

          {/* Submit button */}
          <Button
            variant="contained"
            fullWidth
            onClick={handleEmailSubmit}
            disabled={!email} // Disable the button if email is empty
          >
            Submit
          </Button>
        </Box>
      </Modal>
    </Box>
  );
};

export default Home;
