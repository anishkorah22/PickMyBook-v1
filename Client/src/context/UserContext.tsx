import React, { createContext, useContext, useState, ReactNode, useEffect } from 'react';
import { useQuery, gql } from '@apollo/client';

// Define the shape of user data
interface UserContextType {
  userId: string | null;
  email: string | null;
  roles: string[] | null;
  setUserId: (id: string | null) => void;
  setEmail: (email: string | null) => void;
  setRoles: (roles: string[] | null) => void;
}

// Create the UserContext with default values
const UserContext = createContext<UserContextType | undefined>(undefined);

// Custom hook to use the UserContext
export const useUserContext = () => {
  const context = useContext(UserContext);
  if (!context) {
    throw new Error('useUserContext must be used within a UserProvider');
  }
  return context;
};

// GraphQL query to get user details
const GET_USER = gql`
  query GetUser($id: Int!) {
    user(id: $id) {
      userId
      userName
      isDeleted
      roles
    }
  }
`;

// Provider component to wrap around the app
export const UserProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [userId, setUserId] = useState<string | null>(null);
  const [email, setEmail] = useState<string | null>(null);
  const [roles, setRoles] = useState<string[] | null>(null);

  // Fetch userId from localStorage when the app initializes
  useEffect(() => {
    const storedUserId = localStorage.getItem('userId');
    if (storedUserId) {
      setUserId(storedUserId);
    }
  }, []);

  // If userId is available, run the GraphQL query to fetch user data
  const { data, loading, error } = useQuery(GET_USER, {
    variables: { id: userId ? parseInt(userId) : 0 },
    skip: !userId, // Skip the query if userId is not available
  });

  useEffect(() => {
    if (data && data.user) {
      setEmail(data.user.userName);
      setRoles(data.user.roles);
    }
  }, [data]);

  if (loading) return <p>Loading user data...</p>;
  if (error) return <p>Error loading user data</p>;

  // Provide the user data and setters via context
  return (
    <UserContext.Provider value={{ userId, email, roles, setUserId, setEmail, setRoles }}>
      {children}
    </UserContext.Provider>
  );
};
