import { PublicClientApplication } from '@azure/msal-browser';

const clientId = import.meta.env.VITE_AZURE_CLIENT_ID;
const applicationIdUri = import.meta.env.VITE_APPLICATION_ID_URI;
const redirectUri = import.meta.env.VITE_APPLICATION_REDIRECT_URI;

const loginRequest = {
  scopes: ['openid', 'profile', 'email', `api://${applicationIdUri}/user_impersonation`],
};

const msalConfig = {
  auth: {
    clientId,
    authority: 'https://login.microsoftonline.com/common',
    redirectUri,
  },
  cache: {
    cacheLocation: 'sessionStorage',
    storeAuthStateInCookie: false,
  },
};

const msalInstance = new PublicClientApplication(msalConfig);

export { loginRequest, msalConfig, msalInstance };
