import { Box, Card, Typography } from "@mui/material";
import MenuBookIcon from '@mui/icons-material/MenuBook';
import { gql, useQuery } from "@apollo/client";

const GET_DASHBOARD_COUNTS = gql`
  {
    dashboardCounts {
      totalCurrentBorrowTransactions
      totalBooks
      totalActiveUsers
    }
  }
`;

const DashboardCounts = () => {
  // Extract data, loading, and error from the useQuery hook
  const { loading, error, data } = useQuery(GET_DASHBOARD_COUNTS);

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error.message}</p>;

  // Destructure the values from the GraphQL response
  const { totalActiveUsers, totalBooks, totalCurrentBorrowTransactions } = data.dashboardCounts;

  return (
    <>
      <Box sx={{ m: 13 }}>
        <Box sx={{ display: "flex" }}>
          <Card sx={{ ml: 10, bgcolor: "#e8d9d3", height: 120, width: 220, display: "flex", justifyContent: "space-evenly", alignItems: "center" }}>
            <MenuBookIcon sx={{ color: "#ffff", bgcolor: "#d0977f", fontSize: 60 }} />
            <Box>
              <Typography>Total Users</Typography>
              <Typography><b>{totalActiveUsers}</b></Typography>
            </Box>
          </Card>
          <Card sx={{ ml: 10, bgcolor: "#e8d9d3", height: 120, width: 220, display: "flex", justifyContent: "space-evenly", alignItems: "center" }}>
            <MenuBookIcon sx={{ color: "#ffff", bgcolor: "#d0977f", fontSize: 60 }} />
            <Box>
              <Typography>Total Books</Typography>
              <Typography><b>{totalBooks}</b></Typography>
            </Box>
          </Card>
          <Card sx={{ ml: 10, bgcolor: "#e8d9d3", height: 120, width: 220, display: "flex", justifyContent: "space-evenly", alignItems: "center" }}>
            <MenuBookIcon sx={{ color: "#ffff", bgcolor: "#d0977f", fontSize: 60 }} />
            <Box>
              <Typography>Borrowed Books</Typography>
              <Typography><b>{totalCurrentBorrowTransactions}</b></Typography>
            </Box>
          </Card>
        </Box>
      </Box>
    </>
  );
};

export default DashboardCounts;
