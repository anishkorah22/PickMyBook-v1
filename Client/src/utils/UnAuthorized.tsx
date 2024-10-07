import { Box, Typography } from "@mui/material";
import React from "react";
import { Link } from "react-router-dom";

const UnAuthorized: React.FC = () => {
  return (
    <Box>
      <h1>Unauthorized Access</h1>
      <Typography>You do not have permission to view this page.</Typography>
      <Link to="/">Go back to the homepage</Link>
    </Box>
  );
};

export default UnAuthorized;
