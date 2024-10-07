import React, { useState } from "react";
import {
  Box,
  Grid,
  Card,
  CardMedia,
  CardContent,
  Typography,
  Button,
  TextField,
  IconButton,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
} from "@mui/material";
import SearchIcon from "@mui/icons-material/Search";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import { useQuery, useMutation, gql } from "@apollo/client";
import { useUserContext } from '../../context/UserContext';


// GraphQL Queries and Mutations
const GET_BOOKS = gql`
  query GetBooks {
    books {
      bookId
      title
      author
      publisher
      genre
      availableCopies
      imageUrls
      isbn
    }
  }
`;

const CREATE_BORROW_REQUEST = gql`
  mutation createBorrowRequest($bookId: Int!, $userId: Int!) {
    createBorrowRequest(bookId: $bookId, userId: $userId) {
      userId,
      bookId
    }
  }
`;

interface Book {
  bookId: number;
  title: string;
  author: string;
  publisher: string;
  genre: string;
  availableCopies: number;
  imageUrls: string[];
  isbn: string;
}

// UserBookCatalogue component
const UserBookCatalogue: React.FC = () => {
  const [searchQuery, setSearchQuery] = useState("");
  const [isSearchActive, setIsSearchActive] = useState(false);
  const [open, setOpen] = useState(false);
  const [selectedBook, setSelectedBook] = useState<Book | null>(null);
  
  const {userId } = useUserContext(); // Access user details

  // Fetch books from GraphQL API
  const { loading, error, data } = useQuery(GET_BOOKS);

  // Borrow mutation
  const [createBorrowRequest] = useMutation(CREATE_BORROW_REQUEST, {
    onCompleted: (data) => {
      console.log("Borrow Request Created:", data);
      handleClose();
    },
    onError: (err) => {
      console.error("Error creating borrow request:", err);
    }
  });

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error fetching books!</p>;

  const books: Book[] = data.books;

  const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearchQuery(e.target.value);
    setIsSearchActive(e.target.value !== ""); // Track if a search is active
  };

  const handleClearSearch = () => {
    setSearchQuery("");
    setIsSearchActive(false);
  };

  const handleClickOpen = (book: Book) => {
    setSelectedBook(book); // Set the selected book details
    setOpen(true); // Open the modal
  };

  const handleClose = () => {
    setOpen(false); // Close the modal
    setSelectedBook(null); // Clear selected book details
  };

  const handleConfirmBorrow = () => {
    if (selectedBook) {
      // Call the mutation with bookId and userId
      createBorrowRequest({
        variables: {
          bookId: selectedBook.bookId,
          userId: parseInt(userId, 10),
        }
      });
    }
  };

  const filteredBooks = books.filter(
    (book) =>
      book.title.toLowerCase().includes(searchQuery.toLowerCase()) ||
      book.author.toLowerCase().includes(searchQuery.toLowerCase()) ||
      book.publisher.toLowerCase().includes(searchQuery.toLowerCase()) ||
      book.genre.toLowerCase().includes(searchQuery.toLowerCase())
  );

  return (
    <Box sx={{ ml: 15, mt: 15, mr: 5 }}>
      <Box sx={{ display: "flex", justifyContent: "center", alignItems: "center", mb: 5 }}>
        {isSearchActive && (
          <IconButton onClick={handleClearSearch} sx={{ mr: 1 }}>
            <ArrowBackIcon />
          </IconButton>
        )}
        <TextField
          sx={{
            position: "fixed",
            width: "30em",
            backgroundColor: "rgba(255,255,255)",
            zIndex: 500,
          }}
          placeholder="   Search..."
          variant="outlined"
          value={searchQuery}
          onChange={handleSearchChange}
          InputProps={{
            startAdornment: (
              <span role="img" aria-label="search">
                <SearchIcon />
              </span>
            ),
          }}
        />
      </Box>
      <Box sx={{ ml: 18 }}>
        <Grid container spacing={3}>
          {filteredBooks.map((book) => (
            <Grid item xs={12} sm={6} md={4} key={book.title}>
              <Card sx={{ display: "flex", flexDirection: "row", height: 300 }}>
                <CardMedia
                  component="img"
                  image={book.imageUrls?.[0] || "src/assets/DefaultBook.png"} // Fallback to a default image
                  alt={book.title}
                  sx={{ height: 200, p: 1 }}
                />
                <CardContent>
                  <Typography variant="h6" gutterBottom>
                    {book.title}
                  </Typography>
                  <Typography variant="subtitle1" color="text.secondary">
                    {book.author}
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    {book.publisher}
                  </Typography>
                  <Typography variant="subtitle1" color="text.secondary">
                    {book.genre}
                  </Typography>
                  <Typography
                    variant="body2"
                    color={book.availableCopies > 0 ? "green" : "red"}
                    sx={{ mb: 2 }}
                  >
                    {book.availableCopies > 0
                      ? `${book.availableCopies} Copies Available`
                      : "Not Available"}
                  </Typography>
                  <Button
                    sx={{
                      bgcolor: "#d0977f",
                      "&:hover": {
                        bgcolor: "#ca836b",
                      },
                    }}
                    variant="contained"
                    disabled={book.availableCopies === 0}
                    onClick={() => handleClickOpen(book)} // Open modal on click
                  >
                    Borrow
                  </Button>
                </CardContent>
              </Card>
            </Grid>
          ))}
        </Grid>
      </Box>

      {/* Modal for borrowing the book */}
      <Dialog open={open} onClose={handleClose}>
        <DialogTitle>Borrow Book</DialogTitle>
        <DialogContent sx={{ p: 10 }}>
          {selectedBook && (
            <>
              <Typography variant="body2" color="text.secondary">
                BookTitle : {selectedBook.title}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Author : {selectedBook.author}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Publisher : {selectedBook.publisher}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                ISBN: {selectedBook.isbn}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Genre : {selectedBook.genre}
              </Typography>
            </>
          )}
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose} color="primary">
            Cancel
          </Button>
          <Button
            sx={{ bgcolor: "#d0977f" }}
            onClick={handleConfirmBorrow} // Trigger the borrow mutation
            variant="contained"
            disabled={selectedBook?.availableCopies === 0}
          >
            Confirm Borrow
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
};

export default UserBookCatalogue;
