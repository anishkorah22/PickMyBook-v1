import React from 'react';
import { Navigate } from 'react-router-dom';

// Helper function to check if token exists in local storage
const useAuth = () => {
  const token = localStorage.getItem('jwtToken');
  return token !== null;
};

const ProtectedRoute = ({ children }: { children: JSX.Element }) => {
  const isAuthenticated = useAuth();

  if (!isAuthenticated) {
    // If user is not authenticated, redirect to login page
    return <Navigate to="/login" />;
  }

  // If user is authenticated, render the child component (protected route)
  return children;
};

export default ProtectedRoute;
