import { useContext } from 'react';

import { MainLayoutContext, MainLayoutContextValues } from '@/layouts/main/context';

const useMainLayoutContext = (): MainLayoutContextValues => {
    const context = useContext(MainLayoutContext);
    if (!context) {
        throw new Error('useMainLayoutContext must be used within a MainLayoutProvider');
    }
    return context;
};

export { useMainLayoutContext };
