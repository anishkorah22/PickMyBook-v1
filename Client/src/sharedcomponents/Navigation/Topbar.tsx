import { Box } from '@mui/material'
import React from 'react'

const Topbar = () => {
  return (
    <>
    <Box
    sx={{
        height: '4vh',
        width: '100%',
        position: 'fixed',
        top: '0%',
        left: '0',
        zIndex: 1000,
        color: 'white',
        background: '#D0977F',
        padding: '20px',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        boxShadow: '0px 0px 10px rgba(0,0,0,0.2)',
  
    }}>

        
    </Box>
      
    </>
  )
}

export default Topbar;
