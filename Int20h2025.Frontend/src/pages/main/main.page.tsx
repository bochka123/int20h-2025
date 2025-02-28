import { FC } from 'react';

import { ConnectionContext } from '@/context/hub';
import { useConnection } from '@/hooks';
import { MainLayout } from '@/layouts';

const MainPage: FC = () => {
    const { connection } = useConnection();
    
    return (
        <ConnectionContext.Provider value={connection}>
            <MainLayout />
        </ConnectionContext.Provider>
    );
};

export { MainPage };
