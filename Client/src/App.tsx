import './App.css';
import React, { useEffect, useState } from 'react';
import StaffDashboard from './pages/Dashboard/StaffDashboard';
import { ApolloClient, ApolloProvider, InMemoryCache, useQuery, gql } from '@apollo/client';
import SideBar from './sharedcomponents/Navigation/SideBar';
import Topbar from './sharedcomponents/Navigation/Topbar';
import { BrowserRouter as Router, Route, Routes, useLocation } from 'react-router-dom';
import BookCatalogueTable from './components/BookCatalogue/BookCatalogueTable';
import UserTable from './components/Users/UserTable';
import createUploadLink from 'apollo-upload-client/createUploadLink.mjs';
import UserBookCatalogue from './components/BookCatalogue/UserBookCatalogue';
import Home from './pages/HomePage/Home';
import { UserProvider } from './context/UserContext';
import UserDashboard from './pages/Dashboard/UserDashboard';

// Apollo client setup
const client = new ApolloClient({
  cache: new InMemoryCache(),
  link: createUploadLink({ uri: 'https://localhost:7131/graphql/' }),
});

// GraphQL query to fetch logged-in user's details
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

const App: React.FC = () => {
  const [userId, setUserId] = useState<string | null>(null); // State to hold userId

  // Fetch userId from localStorage if available
  useEffect(() => {
    const storedUserId = localStorage.getItem('userId');
    if (storedUserId) setUserId(storedUserId);
  }, []);

  return (
    <ApolloProvider client={client}>
      <Router>
        <MainApp userId={userId} setUserId={setUserId} />
      </Router>
    </ApolloProvider>
  );
};

const MainApp: React.FC<{ userId: string | null; setUserId: (id: string) => void }> = ({ userId }) => {
  const [email, setEmail] = useState<string | null>(null);
  const [roles, setRoles] = useState<string[] | null>(null);
  const location = useLocation(); // Get current route location

  // Use Apollo Client's useQuery hook to fetch user data
  const { data, loading, error } = useQuery(GET_USER, {
    variables: { id: userId ? parseInt(userId) : 0 },
    skip: !userId, // Skip query if userId is null
  });

  // Ensure correct access to data from the GraphQL query
  useEffect(() => {
    if (data && data.user) {
      setEmail(data.user.userName); // Corrected from data.email
      setRoles(data.user.roles);    // Corrected from data.roles
    }
  }, [data]);

  // Conditionally render SideBar and Topbar except for Home and Login pages
  const shouldShowNav = location.pathname !== '/' && location.pathname !== '/login';

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error loading user data!</p>;

  // Define role-based routes
  const renderRoutes = () => {
    if (roles && roles.includes('admin')) {
      return (
        <>
          <Route path="/dashboard" element={<StaffDashboard />} />
          <Route path="/book-catalogue" element={<BookCatalogueTable />} />
          <Route path="/users" element={<UserTable />} />
        </>
      );
    }

    if (roles && roles.includes('Staff')) {
      return (
        <>
          <Route path="/dashboard" element={<StaffDashboard />} />
          <Route path="/book-catalogue" element={<BookCatalogueTable />} />
          <Route path="/users" element={<UserTable />} />

          {/* Add or modify routes as needed for staff */}
        </>
      );
    }

    if (roles && roles.includes('User')) {
      return (
        <>
          <Route path="/dashboard" element={<UserDashboard />} />
          <Route path="/book-catalogue" element={<UserBookCatalogue />} />
        </>
      );
    }

    return null; // Fallback if no role matches
  };

  return (
    <UserProvider>
      {/* Conditionally render Topbar and SideBar */}
      {shouldShowNav && <Topbar />}
      {shouldShowNav && <SideBar />}

      {/* Main content */}
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Home />} />
        {renderRoutes()}
      </Routes>
    </UserProvider>
  );
};

export default App;
