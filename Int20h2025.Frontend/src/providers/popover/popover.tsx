import { FC, useState } from 'react';

import { PopoverContext } from '@/context/popover';

import { PopoverProviderType } from './types';

const PopoverProvider: FC<PopoverProviderType> = ({ children }) => {

    const [popoverState, setPopoverState] = useState<boolean>(false);

    const popoverProviderValue = { popoverState, setPopoverState };

    return (
        <PopoverContext.Provider value={popoverProviderValue}>{children}</PopoverContext.Provider>
    );
};

export { PopoverProvider };
