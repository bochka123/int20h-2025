import { FC } from 'react';
import { Outlet } from 'react-router-dom';

import { ConnectionContext } from '@/context/hub';
import { useConnection } from '@/hooks';

const MainPage: FC = () => {
    const { connection } = useConnection();
    
    return (
        <ConnectionContext.Provider value={connection}>
            <Outlet />
        </ConnectionContext.Provider>
    );
};

export { MainPage };
