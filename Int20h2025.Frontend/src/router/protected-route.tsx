import { FC } from 'react';
import { Outlet } from 'react-router-dom';

// import { Loader } from '@/components';
// import { useGetMyProfileQuery } from '@/services';

const ProtectedRoute: FC = () => {
    // const { data: profileData, isLoading: isProfileLoading } = useGetMyProfileQuery();

    return (
        // isProfileLoading
        //     ? <Loader />
            // : profileData?.data.id
            //     ?
            <Outlet />
            // : <Navigate to={'/auth'} />
    );
};

export { ProtectedRoute };
