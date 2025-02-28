import { useMemo } from 'react';
import { useSelector } from 'react-redux';

import { selectCurrentId } from '@/store/auth';

const useProfileHook = (): unknown => {
    const id = useSelector(selectCurrentId);

    return useMemo(() => ({
        id,
    }), [
        id,
    ]);
};

export { useProfileHook };
