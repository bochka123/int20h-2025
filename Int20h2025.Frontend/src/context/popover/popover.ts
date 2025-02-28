import React, { createContext } from 'react';

type PopoverContextType = { 
    popoverState: boolean; 
    setPopoverState: React.Dispatch<React.SetStateAction<boolean>>;
}

const initialState: PopoverContextType = {
    popoverState: false,
    setPopoverState: () => {},
};

export const PopoverContext = createContext<PopoverContextType>(initialState);
