import { Box } from '@mui/material'
import React from 'react'
import DashboardCounts from '../../components/Dashboard/DashboardCounts'
import DashboardTable from '../../components/Dashboard/DashboardTable'


const StaffDashboard = () => {
  return (
    <>
      <Box sx={{ml: "9em"}}>
        <Box
        sx={{
            width:'100%',
            bgcolor: 'background.light',
        }}
        >
            <DashboardCounts/>
            <DashboardTable/>        
        </Box>
      </Box>
    </>
  )
}

export default StaffDashboard
