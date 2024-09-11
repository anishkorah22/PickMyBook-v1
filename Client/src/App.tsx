import './App.css'
import StaffDashboard from './pages/Dashboard/StaffDashboard'
import { ApolloClient, ApolloProvider, InMemoryCache } from '@apollo/client'
import SideBar from './sharedcomponents/Navigation/SideBar';
import Topbar from './sharedcomponents/Navigation/Topbar';
import { Box } from '@mui/material';

const client = new ApolloClient({
  uri: 'https://localhost:7131/graphql', // Replace with your actual GraphQL endpoint
  cache: new InMemoryCache(),
});
function App() {

  return (
    < >

    <Topbar/>
     <SideBar />
      <ApolloProvider client={client}>
       <StaffDashboard/>
      </ApolloProvider>

    </>
  )
}

export default App
