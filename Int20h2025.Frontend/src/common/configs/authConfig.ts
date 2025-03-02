import { PublicClientApplication } from '@azure/msal-browser';

const loginRequest = {
  scopes: [import.meta.env.VITE_AZURE_SCOPES],
};

const msalConfig = {
  auth: {
    clientId: import.meta.env.VITE_AZURE_CLIENT_ID,
    authority: import.meta.env.VITE_AZURE_AUTHORITY,
    redirectUri: import.meta.env.VITE_APPLICATION_REDIRECT_URI
  },
  cache: {
    cacheLocation: 'sessionStorage',
    storeAuthStateInCookie: false,
  },
};

const msalInstance = new PublicClientApplication(msalConfig);

export { loginRequest, msalConfig, msalInstance };
