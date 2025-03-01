import { FC } from 'react';
import { Navigate, Outlet } from 'react-router-dom';

import { useProfileHook } from '@/hooks';

const ProtectedRoute: FC = () => {
    const { id } = useProfileHook();

    return (
        id
            ? <Outlet />
            : <Navigate to={'/auth'} />
    );
};

export { ProtectedRoute };
