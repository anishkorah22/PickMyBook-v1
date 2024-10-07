import { gql, useMutation, useQuery } from '@apollo/client';
import {
  Box,
  Button,
  Switch,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  TextField,
  IconButton,
  Typography,
} from '@mui/material';
import { useState, useEffect } from 'react';
import BorderColorRoundedIcon from '@mui/icons-material/BorderColorRounded';

interface Book {
  genre: string;
  publishedYear: number;
  title: string;
  author: string;
  isbn: string;
  publisher: string;
  availableCopies: number;
  isDeleted: boolean;
  bookId: number;
}

interface BooksData {
  AllBooks: Book[];
}

interface UpdateBookVariables {
  book: {
    bookId: number;
    title: string;
    author: string;
    isbn: string;
    publisher: string;
    availableCopies: number;
  }
}

interface BookStatusChangedData {
  bookStatusChanged: {
    bookId: number;
    isDeleted: boolean;
  };
}


const GET_BOOK_CATALOGUE = gql`
  {
    AllBooks {
      title
      author
      isbn
      publisher
      availableCopies
      isDeleted
      bookId
    }
  }
`;


const ADD_BOOK = gql`
  mutation addBook($book: BookInput!) {
    addBook(book: $book) {
      title
      author
      isbn
      publisher
      availableCopies
      publishedYear
      genre
    } 
  }
`;


const UPDATE_BOOK = gql`
  mutation updateBook($book: BookInput!) {
    updateBook(book: $book) {
      bookId
      title
      author
      isbn
      publisher
      availableCopies
    }
  }
`;

const UPDATE_BOOK_STATUS = gql`
  mutation updateBookStatus($bookId: Int!, $isDeleted: Boolean!) {
    updateBookStatus(bookId: $bookId, isDeleted: $isDeleted) {
      isDeleted
    }
  }
`;

const BOOK_UPDATED_SUBSCRIPTION = gql`
  subscription {
    bookStatusChanged {
      bookId
      isDeleted
    }
  }
`;

const BookCatalogueTable = () => {
  const [booksData, setBooksData] = useState<Book[]>([]);
  const [selectedBook, setSelectedBook] = useState<Book | null>(null);
  const [newBook, setNewBook] = useState<Book | null>(null);
  const [isAdding, setIsAdding] = useState(false);
  //const [addBook] = useMutation<{ addBook: Book }, { book: Book, files: File[] }>(ADD_BOOK);
    const [updateBookStatus] = useMutation<void, { bookId: number; isDeleted: boolean }>(UPDATE_BOOK_STATUS);
  const [updateBook] = useMutation<{ updateBook: Book }, UpdateBookVariables>(UPDATE_BOOK);
  const [selectedFiles, setSelectedFiles] = useState<File[]>([]);
  const [bulkUploadOpen, setBulkUploadOpen] = useState(false);
  const [selectedFile, setSelectedFile] = useState<File | null>(null);


  const [open, setOpen] = useState(false);

  const { loading, error, subscribeToMore } = useQuery<BooksData>(GET_BOOK_CATALOGUE, {
    onCompleted: (data) => {
      setBooksData(data.AllBooks);
    },
  });

  useEffect(() => {
    const unsubscribe = subscribeToMore<BookStatusChangedData>({
      document: BOOK_UPDATED_SUBSCRIPTION,
      updateQuery: (prev, { subscriptionData }) => {
        if (!subscriptionData.data) return prev;
        const updatedBook = subscriptionData.data.bookStatusChanged;
        setBooksData((prevBooks) =>
          prevBooks.map((book) =>
            book.bookId === updatedBook.bookId
              ? { ...book, isDeleted: updatedBook.isDeleted }
              : book
          )
        );
        return prev;
      },
    });
    return () => unsubscribe();
  }, [subscribeToMore]);

  const handleBulkUploadClickOpen = () => setBulkUploadOpen(true);
  const handleBulkUploadClose = () => setBulkUploadOpen(false);

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error.message}</p>;

  const handleToggle = async (book: Book) => {
    try {
      await updateBookStatus({
        variables: { bookId: book.bookId, isDeleted: !book.isDeleted },
      });
      setBooksData((prevBooks) =>
        prevBooks.map((b) =>
          b.bookId === book.bookId ? { ...b, isDeleted: !book.isDeleted } : b
        )
      );
    } catch (e) {
      console.error('Error updating book status:', e);
    }
  };

  const handleEditClick = (book: Book) => {
    setSelectedBook(book);
    setIsAdding(false); 
    setOpen(true);
  };

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files) {
      // Convert FileList to an array of File objects
      const filesArray = Array.from(e.target.files);
      setSelectedFiles(filesArray);
    }
  };

  const handleAddClick = () => {
    setNewBook({
      title: '',
      author: '',
      isbn: '',
      publisher: '',
      availableCopies: 0,
      isDeleted: false,
      publishedYear: 0,
      genre: '',
      bookId: 0,
    });
    setIsAdding(true); // Set mode to adding
    setOpen(true);
  };
   const handleBulkUpload = async () => {
    if (!selectedFile) return;

    const formData = new FormData();
    formData.append('file', selectedFile);

    try {
      const response = await fetch('https://localhost:7131/api/FileUpload/upload-books', {
        method: 'POST',
        body: formData,
      });

      if (response.ok) {
        const result = await response.json();
        console.log('Bulk upload successful:', result);
        handleBulkUploadClose();
        refetch(); // Refetch users after bulk upload
      } else {
        console.error('Bulk upload failed');
      } 
    } catch (error) {
      console.error('Error bulk uploading:', error);
    }
  };
  const handleClose = () => {
    setOpen(false);
  };

  const handleSave = async () => {
    try {
      if (isAdding && newBook) {
        const formData = new FormData();
        //Create variables for the GraphQL mutation
        const variables = {
          book: {
            ...newBook,
            publishedYear: newBook.publishedYear || new Date().getFullYear(),
            genre: newBook.genre || 'Unknown',
          },
        };
  
        formData.append(
          'operations',
          JSON.stringify({
            query: ADD_BOOK.loc?.source.body,
            variables
          })
        );
    
        // Send the request to the GraphQL API
        const response = await fetch('https://localhost:7131/graphql', {
          method: 'POST',
          body: formData,
        });
  
        const result = await response.json();
  
        if (result.errors) {
          throw new Error(result.errors[0].message);
        }
  
        // Update the books list with the newly added book
        setBooksData((prevBooks) => [...prevBooks, result.data.addBook]);
      } else if (selectedBook) {
        // Handle updating an existing book
        await updateBook({
          variables: {
            book: {
              bookId: selectedBook.bookId,
              title: selectedBook.title,
              author: selectedBook.author,
              isbn: selectedBook.isbn,
              publisher: selectedBook.publisher,
              availableCopies: selectedBook.availableCopies,
            },
          },
        });
  
        setBooksData((prevBooks) =>
          prevBooks.map((book) =>
            book.bookId === selectedBook.bookId ? selectedBook : book
          )
        );
      }
  
      handleClose(); // Close the dialog
    } catch (e) {
      console.error('Error saving book:', e);
      // Optionally handle error, e.g., display notification
    }
  };

  return (
    <>
      <Box sx={{ mt: 15, ml: 30 }}>
        <Button sx={{ bgcolor: '#d9a07c', color: '#fff', marginRight: "15px" }} onClick={handleAddClick}>
          Add Book
        </Button>
        <Button
            sx={{ backgroundColor: "#d9a07c" }}
            variant="contained"
            onClick={handleBulkUploadClickOpen}
          >
            Bulk Upload
          </Button>               
      </Box>
      <Box sx={{ marginTop: 7, marginLeft: 30 }}>
        <TableContainer sx={{ maxHeight: 300 }}>
          <Table stickyHeader sx={{ maxWidth: 1000 }} size="small" aria-label="a dense table">
            <TableHead >
              <TableRow>
                <TableCell sx={{backgroundColor:"#e3c2b6"}} >Sl No.</TableCell>
                <TableCell sx={{backgroundColor:"#e3c2b6"}} align="right">Book Title</TableCell>
                <TableCell sx={{backgroundColor:"#e3c2b6"}} align="right">Author</TableCell>
                <TableCell sx={{backgroundColor:"#e3c2b6"}} align="right">ISBN</TableCell>
                <TableCell sx={{backgroundColor:"#e3c2b6"}} align="right">Publisher</TableCell>
                <TableCell sx={{backgroundColor:"#e3c2b6"}} align="right">Copies</TableCell>
                <TableCell sx={{backgroundColor:"#e3c2b6"}} align="right">Active/Not</TableCell>
                <TableCell sx={{backgroundColor:"#e3c2b6"}} align="right">Update</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {booksData.map((book, index) => (
                <TableRow
                  key={book.bookId}
                  sx={{
                    '&:last-child td, &:last-child th': { border: 0 },
                    bgcolor: '#ffffff',
                  }}
                >
                  <TableCell component="th" scope="row">
                    {index + 1}
                  </TableCell>
                  <TableCell align="right">{book.title}</TableCell>
                  <TableCell align="right">{book.author}</TableCell>
                  <TableCell align="right">{book.isbn}</TableCell>
                  <TableCell align="right">{book.publisher}</TableCell>
                  <TableCell align="right">{book.availableCopies}</TableCell>
                  <TableCell align="right">
                    <Switch
                      checked={!book.isDeleted}
                      onChange={() => handleToggle(book)}
                      color="warning"
                    />
                  </TableCell>
                  <TableCell align="right">
                    <BorderColorRoundedIcon
                      onClick={() => handleEditClick(book)}
                      sx={{ cursor: 'pointer', color: '#d0977f' }}
                    />
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      </Box>

      <Dialog open={open} onClose={handleClose}>
        <DialogTitle>{isAdding ? 'Add Book' : 'Update Book'}</DialogTitle>
        <DialogContent>
          {selectedBook && !isAdding && (
            <>
              <TextField
                autoFocus
                margin="dense"
                label="Title"
                type="text"
                variant="standard"
                value={isAdding ? newBook?.title : selectedBook?.title}
                onChange={(e) => {
                  if (isAdding && newBook) {
                    setNewBook({ ...newBook, title: e.target.value });
                  } else if (selectedBook) {
                    setSelectedBook({ ...selectedBook, title: e.target.value });
                  }
                }}
              />
              <TextField
                autoFocus
                margin="dense"
                label="Author"
                type="text"
                fullWidth
                variant="standard"
                value={selectedBook.author}
                onChange={(e) =>
                  setSelectedBook({ ...selectedBook, author: e.target.value })
                }
              />
              <TextField
                autoFocus
                margin="dense"
                label="ISBN"
                type="text"
                fullWidth
                variant="standard"
                value={selectedBook.isbn}
                onChange={(e) =>
                  setSelectedBook({ ...selectedBook, isbn: e.target.value })
                }
              />
              <TextField
                autoFocus
                margin="dense"
                label="Publisher"
                type="text"
                fullWidth
                variant="standard"
                value={selectedBook.publisher}
                onChange={(e) =>
                  setSelectedBook({ ...selectedBook, publisher: e.target.value })
                }
              />
              <TextField
                autoFocus
                margin="dense"
                label="Available Copies"
                type="number"
                fullWidth
                variant="standard"
                value={selectedBook.availableCopies}
                onChange={(e) =>
                  setSelectedBook({ ...selectedBook, availableCopies: Number(e.target.value) })
                }
              />
            </>
          )}

          {isAdding && (
            <>
              <TextField
                autoFocus
                margin="dense"
                label="Title"
                type="text"
                fullWidth
                variant="standard"
                value={newBook?.title || ''}
                onChange={(e) =>
                  setNewBook({ ...newBook!, title: e.target.value })
                }
              />
              <TextField
                autoFocus
                margin="dense"
                label="Author"
                type="text"
                fullWidth
                variant="standard"
                value={newBook?.author || ''}
                onChange={(e) =>
                  setNewBook({ ...newBook!, author: e.target.value })
                }
              />
              <TextField
                autoFocus
                margin="dense"
                label="ISBN"
                type="text"
                fullWidth
                variant="standard"
                value={newBook?.isbn || ''}
                onChange={(e) =>
                  setNewBook({ ...newBook!, isbn: e.target.value })
                }
              />
              <TextField
                autoFocus
                margin="dense"
                label="Publisher"
                type="text"
                fullWidth
                variant="standard"
                value={newBook?.publisher || ''}
                onChange={(e) =>
                  setNewBook({ ...newBook!, publisher: e.target.value })
                }
              />
              <TextField
                autoFocus
                margin="dense"
                label="Available Copies"
                type="number"
                fullWidth
                variant="standard"
                value={newBook?.availableCopies || 0}
                onChange={(e) =>
                  setNewBook({ ...newBook!, availableCopies: Number(e.target.value) })
                }
              />
              <TextField
                autoFocus
                margin="dense"
                label="Published Year"
                type="number"
                fullWidth
                variant="standard"
                value={newBook?.publishedYear || new Date().getFullYear()}
                onChange={(e) =>
                  setNewBook({ ...newBook!, publishedYear: Number(e.target.value) })
                }
              />
              <TextField
                autoFocus
                margin="dense"
                label="Genre"
                type="text"
                fullWidth
                variant="standard"
                value={newBook?.genre || ''}
                onChange={(e) =>
                  setNewBook({ ...newBook!, genre: e.target.value })
                }
              />
              {/* <TextField
                    margin="dense"
                    label="Files"
                    type="file"
                    variant="standard"
                    onChange={handleFileChange} // Use the handleFileChange function here
                /> */}
            </>
          )}
          
{/* 
          <input
                type="file"
                multiple
                onChange={handleFileChange}
                accept=".jpg,.jpeg,.png"
              /> */}
        </DialogContent>
        <DialogActions>
        <Button onClick={handleClose} color="primary">Cancel</Button>
          <Button onClick={handleSave} color="primary">{isAdding ? 'Add' : 'Save'}</Button>
          </DialogActions>
      </Dialog>
      <Dialog open={bulkUploadOpen} onClose={handleBulkUploadClose}>
        <DialogTitle>
          Bulk Upload
          <IconButton
            edge="end"
            color="inherit"
            onClick={handleBulkUploadClose}
            aria-label="close"
            sx={{ position: 'absolute', right: 8, top: 8 }}
          >
          </IconButton>
        </DialogTitle>
        <DialogContent>
          <Typography gutterBottom>Upload your file here</Typography>
          <Box sx={{border:"1px black dashed",h:50, borderRadius:3}}>
          <input
            type="file"
            accept=".xlsx, .xls"
            onChange={(e) => {
              if (e.target.files?.[0]) {
                setSelectedFile(e.target.files[0]);
              }
            }}
            style={{ padding:30 }}
          />
          </Box>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleBulkUploadClose}>Cancel</Button>
          <Button
            variant="contained"
            sx={{ backgroundColor: "#d9a07c" }}
            onClick={handleBulkUpload}
            disabled={!selectedFile}
          >
            Upload
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
};

export default BookCatalogueTable;
