import { Box, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@mui/material";
import { useQuery, gql } from '@apollo/client';

const GET_BORROWED_BOOKS = gql`
  {
  borrowings {
    borrowDate
    returnDate
    book {
      title
    }
    user {
      userName
    }
  }
}

`;

const DashboardTable = () => {
  // Destructure loading, error, and data from the useQuery hook
  const { loading, error, data } = useQuery(GET_BORROWED_BOOKS);

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error.message}</p>;

  // Extract borrowings data
  const { borrowings } = data;

  return (
    <>
      <Box sx={{ pl: "11em",pr:"7em"}}>
        <TableContainer sx={{ maxHeight: 200 }}>
          <Table sx={{ minWidth: 650 }} size="small" aria-label="a dense table">
            <TableHead>
              <TableRow sx={{bgcolor:"#e3c2b6"}}>
                <TableCell>Sl No.</TableCell>
                <TableCell align="right">User Name</TableCell>
                <TableCell align="right">Book Title</TableCell>
                <TableCell align="right">Issue Date</TableCell>
                <TableCell align="right">Return Date</TableCell>
              </TableRow>
            </TableHead>

            {/* Table body */}
            <TableBody>
              {borrowings.map((borrowing, index) => (
                <TableRow key={index} sx={{ '&:last-child td, &:last-child th': { border: 0 },bgcolor:"#ffffff" }}>
                  <TableCell component="th" scope="row">{index + 1}</TableCell>
                  <TableCell align="right">{borrowing.user.userName}</TableCell>
                  <TableCell align="right">{borrowing.book.title}</TableCell>
                  <TableCell align="right">{new Date(borrowing.borrowDate).toLocaleDateString()}</TableCell>
                  <TableCell align="right">{new Date(borrowing.returnDate).toLocaleDateString()}</TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      </Box>
    </>
  );
};

export default DashboardTable;
