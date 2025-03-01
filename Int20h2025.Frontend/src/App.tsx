import { MsalProvider } from '@azure/msal-react';
import { FC } from 'react';
import { Provider } from 'react-redux';
import { RouterProvider } from 'react-router-dom';

import { router } from '@/router';
import { store } from '@/store';

import { msalInstance } from './common/configs';
import { PopoverProvider, ToastProvider } from './providers';

const App: FC = () => {

    return (
        <Provider store={store}>
            <MsalProvider instance={msalInstance}>
                <ToastProvider>
                    <PopoverProvider>
                        <RouterProvider router={router} />
                    </PopoverProvider>
                </ToastProvider>
            </MsalProvider>
        </Provider>
    );
};

export default App;
