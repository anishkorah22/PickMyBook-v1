import React from "react";
import { Route, Redirect } from "react-router-dom";
import { getUserRoles } from "./utility"; // JWT decoding function

interface PrivateRouteProps {
  component: React.ComponentType<unknown>;
  roles: string[]; // Roles allowed to access the route
  path: string;
  exact?: boolean;
}

const PrivateRoute: React.FC<PrivateRouteProps> = ({ component: Component, roles, ...rest }) => {
  const token = localStorage.getItem("token"); // Assuming token is stored in localStorage
  const userRoles = getUserRoles(token || "");

  const isAuthorized = roles.some((role) => userRoles.includes(role));

  return (
    <Route
      {...rest}
      render={(props) =>
        isAuthorized ? (
          <Component {...props} />
        ) : (
          <Redirect to="/unauthorized" /> // Redirect to unauthorized page if role check fails
        )
      }
    />
  );
};

export default PrivateRoute;

