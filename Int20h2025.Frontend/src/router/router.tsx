import { createBrowserRouter } from 'react-router-dom';

import { MainLayout } from '@/layouts';
import { AuthPage, MainPage } from '@/pages';
import { ProtectedRoute } from '@/router/protected-route.tsx';

export const router = createBrowserRouter([
    {
        element: <ProtectedRoute />,
        children: [
            {
                element: <MainPage />,
                children: [
                    {
                        path: '/',
                        element: <MainLayout />,
                    }
                ]
            },
        ]
    },
    {
        path: 'auth',
        element: <AuthPage />,
    },
    {
        path: '*',
        element: <h1>Page is not found</h1>,
    }
]);
