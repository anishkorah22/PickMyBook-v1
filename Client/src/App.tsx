import './App.css'
import StaffDashboard from './pages/Dashboard/StaffDashboard'
import { ApolloClient, ApolloProvider, InMemoryCache } from '@apollo/client'

const client = new ApolloClient({
  uri: 'https://localhost:7131/graphql', // Replace with your actual GraphQL endpoint
  cache: new InMemoryCache(),
});
function App() {

  return (
    <>
      <ApolloProvider client={client}>
       <StaffDashboard/>
      </ApolloProvider>
    </>
  )
}

export default App
