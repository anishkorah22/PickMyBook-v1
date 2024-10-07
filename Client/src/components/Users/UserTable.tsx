import React, { useState, useEffect } from 'react';
import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Switch,
  Button,
  TextField,
  Typography,
  Box,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  IconButton,
  MenuItem,
  Select,
} from '@mui/material';
import { useQuery, useMutation, gql } from '@apollo/client';
import CloseIcon from '@mui/icons-material/Close';

// GraphQL queries and mutations
const ADD_USER = gql`
  mutation CreateUser($userName: String!, $roles: [String!]!) {
    createUser(userName: $userName, roles: $roles) {
      userId
      userName
      roles
      isDeleted
    }
  }
`;

const UPDATE_USER_STATUS = gql`
  mutation updateUserStatus($userId: Int!, $isDeleted: Boolean!) {
    updateUserStatus(userId: $userId, isDeleted: $isDeleted) {
      isDeleted
    }
  }
`;

const USER_UPDATED_SUBSCRIPTION = gql`
  subscription {
    userStatusChanged {
      userId
      isDeleted
    }
  }
`;

const GET_USERS = gql`
  query GetUsers {
    AllUsers {
      userId
      userName
      roles
      isDeleted
    }
  }
`;

// Define interfaces for User and Subscription Data
interface User {
  userId: number;
  userName: string;
  roles: string;
  isDeleted: boolean;
}

interface UserStatusChangedData {
  userId: number;
  userStatusChanged: User;
}

const UserTable: React.FC = () => {
  const [userName, setUserName] = useState('');
  const [roles, setRoles] = useState<string[]>([]);
  const [usersData, setUsersData] = useState<User[]>([]);
  const { data, refetch, subscribeToMore } = useQuery<{ AllUsers: User[] }>(GET_USERS);
  const [addUser] = useMutation(ADD_USER);
  const [updateUserStatus] = useMutation(UPDATE_USER_STATUS);

  const [addStaffOpen, setAddStaffOpen] = useState<boolean>(false);
  const [bulkUploadOpen, setBulkUploadOpen] = useState<boolean>(false);
  const [search, setSearch] = useState<string>('');
  const [selectedFile, setSelectedFile] = useState<File | null>(null);

  useEffect(() => {
    const unsubscribe = subscribeToMore<UserStatusChangedData>({
      document: USER_UPDATED_SUBSCRIPTION,
      updateQuery: (prev, { subscriptionData }) => {
        if (!subscriptionData.data) return prev;

        const updatedUser = subscriptionData.data.userStatusChanged;
        setUsersData((prevUsers) =>
          prevUsers.map((user) =>
            user.userId === updatedUser.userId
              ? { ...user, isDeleted: updatedUser.isDeleted }
              : user
          )
        );
        return prev;
      },
    });
    return () => unsubscribe();
  }, [subscribeToMore]);

  useEffect(() => {
    if (data) {
      setUsersData(data.AllUsers);
    }
  }, [data]);

  const handleAddStaffClickOpen = () => {
    setAddStaffOpen(true);
    setUserName('');
    setRoles([]);
  };

  const handleAddStaffClose = () => setAddStaffOpen(false);

  const handleBulkUploadClickOpen = () => setBulkUploadOpen(true);
  const handleBulkUploadClose = () => setBulkUploadOpen(false);

  const handleSearchChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSearch(event.target.value);
  };

  const handleAddUser = async () => {
    try {
      await addUser({ variables: { userName, roles } });
      handleAddStaffClose();
      refetch(); // Refetch users after adding
    } catch (error) {
      console.error('Error adding user:', error);
    }
  };

  const handleBulkUpload = async () => {
    if (!selectedFile) return;

    const formData = new FormData();
    formData.append('file', selectedFile);

    try {
      const response = await fetch('https://localhost:7131/api/FileUpload/upload-users', {
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

  const handleToggle = async (user: User) => {
    try {
      await updateUserStatus({
        variables: { userId: user.userId, isDeleted: !user.isDeleted },
      });

      setUsersData((prevUsers) =>
        prevUsers.map((u) =>
          u.userId === user.userId ? { ...u, isDeleted: !user.isDeleted } : u
        )
      );
    } catch (e) {
      console.error('Error updating user status:', e);
    }
  };

  return (
    <Box style={{ padding: '20px' }}>
      <Typography variant="h4" gutterBottom>
        Users
      </Typography>

      <Box sx={{ display: 'flex', justifyContent: 'space-between', ml: 30 }}>
        <Box>
          <Button
            variant="contained"
            sx={{ marginRight: '20px', backgroundColor: "#d9a07c" }}
            onClick={handleAddStaffClickOpen}
          >
            Add Staff
          </Button>
          <Button
            sx={{ backgroundColor: "#d9a07c" }}
            variant="contained"
            onClick={handleBulkUploadClickOpen}
          >
            Bulk Upload
          </Button>
        </Box>
        <TextField
          label="Search"
          variant="outlined"
          size="small"
          value={search}
          onChange={handleSearchChange}
        />
      </Box>

      <TableContainer component={Paper} sx={{ ml: 30, mt: 5, width: '80%', maxHeight: 340}}>
        <Table stickyHeader>
          <TableHead>
            <TableRow>
              <TableCell sx={{backgroundColor:"#e3c2b6"}} >Sl. No</TableCell>
              <TableCell sx={{backgroundColor:"#e3c2b6"}} >User Name</TableCell>
              <TableCell sx={{backgroundColor:"#e3c2b6"}} >Role</TableCell>
              <TableCell sx={{backgroundColor:"#e3c2b6"}} >Active/Not</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {usersData.map((user, index) => (
              <TableRow key={user.userId}>
                <TableCell>{index + 1}</TableCell>
                <TableCell>{user.userName}</TableCell>
                <TableCell>{user.roles}</TableCell>
                <TableCell align="left">
                  <Switch
                    checked={!user.isDeleted}
                    onChange={() => handleToggle(user)}
                    color="warning"
                  />
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      {/* Add Staff Modal */}
      <Dialog open={addStaffOpen} onClose={handleAddStaffClose}>
        <DialogTitle>
          Add Staff
          <IconButton
            edge="end"
            color="inherit"
            onClick={handleAddStaffClose}
            aria-label="close"
            sx={{ position: 'absolute', right: 8, top: 8 }}
          >
            <CloseIcon />
          </IconButton>
        </DialogTitle>
        <DialogContent sx={{fontFamily:"Montserrat"}}>
          <TextField  
            sx={{ m: 3}}
            label="User Name"
            type="text"
            value={userName}
            onChange={(e) => setUserName(e.target.value)}
          />
          <Select
            sx={{ width:200, m: 3 }}
            label="Roles"
            type="text"
            value={roles.join(', ')} 
            onChange={(e) => setRoles(e.target.value.split(',').map(role => role.trim()))}
          >
            <MenuItem value="Admin">Admin</MenuItem>
            <MenuItem value="Staff">Staff</MenuItem>
            <MenuItem value="User">User</MenuItem>
          </Select>
            
        </DialogContent>
        <DialogActions>
          <Button onClick={handleAddStaffClose}>Cancel</Button>
          <Button onClick={handleAddUser}>
            Add
          </Button>
        </DialogActions>
      </Dialog>

      {/* Bulk Upload Modal */}
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
            <CloseIcon />
          </IconButton>
        </DialogTitle>
        <DialogContent sx={{width:300}}>
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
    </Box>
  );
};

export default UserTable;
