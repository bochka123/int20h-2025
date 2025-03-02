import { useState, useEffect } from "react";
import { useMsal } from "@azure/msal-react";
import { loginRequest } from "@/common";

export const useMsalAuth = () => {
  const { instance, accounts } = useMsal();
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  useEffect(() => {
    const fetchToken = async () => {
      console.log(process.env.NODE_ENV);
      console.log(process.env);
      console.log(process.env.REACT_APP_CLIENT_ID);
    };

    fetchToken();
  }, [accounts, instance]);

  const login = async () => {
    try {
      if (accounts.length > 0) {
        try {
          instance.setActiveAccount(accounts[0]);
          const response = await instance.acquireTokenSilent(loginRequest);
          setIsAuthenticated(true);
          return response.accessToken;
        } catch (ex) {
          console.warn("Silent token acquisition failed, falling back to popup login", ex);
        }
      }

      const response = await instance.loginPopup(loginRequest);
      setIsAuthenticated(true);
      return response.accessToken;
    } catch (error) {
      console.error("Login failed:", error);
      throw error;
    }
  };

  const logout = async () => {
    try {
      await instance.logoutPopup();
      setIsAuthenticated(false);
      localStorage.clear();
    } catch (error) {
      console.error("Logout failed:", error);
    }
  };

  return { isAuthenticated, login, logout };
};
