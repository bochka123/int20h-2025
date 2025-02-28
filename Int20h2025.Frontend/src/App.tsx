import { FC } from 'react';
import { Provider } from 'react-redux';
import { RouterProvider } from 'react-router-dom';

import { router } from '@/router';
import { store } from '@/store';

import { PopoverProvider, ToastProvider } from './providers';

const App: FC = () => {

    return (
        <Provider store={store}>
            <ToastProvider>
                <PopoverProvider>
                    <RouterProvider router={router} />
                </PopoverProvider>
            </ToastProvider>
        </Provider>
    );
};

export default App;
