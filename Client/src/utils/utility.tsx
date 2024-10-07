/* eslint-disable @typescript-eslint/no-explicit-any */
import jwtDecode from "jwt-decode";

export const getUserRoles = (token: string): string[] => {
  if (!token) return [];

  const decodedToken: any = jwtDecode(token);
  return decodedToken.roles || [];
};
