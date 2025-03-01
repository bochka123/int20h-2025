import { FC, useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { Navigate, Outlet } from 'react-router-dom';

import { LoaderPage } from '@/pages';
import { useGetProfileQuery } from '@/services';
import { setProfile } from '@/store/auth';

const ProtectedRoute: FC = () => {
    const { data: profile, isLoading } = useGetProfileQuery();
    const dispatch = useDispatch();

    useEffect(() => {
        if (!isLoading && profile) {
            dispatch(setProfile({ id: profile.data.id }));
        }
    }, [dispatch, isLoading, profile]);

    return (
        isLoading
            ? <LoaderPage />
            : profile?.data.id
                ? <Outlet />
                : <Navigate to={'/auth'} />
    );
};

export { ProtectedRoute };
