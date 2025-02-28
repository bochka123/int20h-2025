import { GoogleOAuthProvider } from '@react-oauth/google';
import { FC } from 'react';

import { AuthButton } from './auth-button.tsx';

const clientId = import.meta.env.VITE_GOOGLE_CLIENT_ID;

const GoogleAuthButton: FC = () => {
    return (
        <GoogleOAuthProvider clientId={clientId}>
            <AuthButton/>
        </GoogleOAuthProvider>
    );
};

export { GoogleAuthButton };
