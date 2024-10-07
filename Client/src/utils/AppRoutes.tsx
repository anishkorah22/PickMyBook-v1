import React from "react";
import { Route, BrowserRouter as Router } from "react-router-dom";
import PrivateRoute from "./PrivateRoute";
import UserDashboard from "../pages/Dashboard/UserDashboard";
import StaffDashboard from "../pages/Dashboard/StaffDashboard";
import { Switch } from "@mui/material";
import BookCatalogueTable from "../components/BookCatalogue/BookCatalogueTable";
import UnAuthorized from "./UnAuthorized";

 const AppRoutes = () => (
     <Router>
         <Switch>
             {/* Public Route */}

             {/* Role-based Private Routes */}
             {/* Admin & Staff */}
             <PrivateRoute path="/admin" component={StaffDashboard} roles={["Admin", "Staff"]} />


             {/* Admin */}

             
             {/* Staff */}
             <Route path="/books" component={BookCatalogueTable} exact />

             {/* User */}
             <PrivateRoute path="/user" component={UserDashboard} roles={["User"]} />

             {/* Unauthorized page */}
             <Route path="/unauthorized" component={UnAuthorized} />
         </Switch>
     </Router>
 );

export default AppRoutes;
