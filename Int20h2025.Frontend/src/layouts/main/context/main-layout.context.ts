/* eslint-disable no-unused-vars */
import { createContext } from 'react';

import { IMessageDto } from '@/models/responses';

interface MainLayoutContextValues {
    recentMessages: IMessageDto[];
    addMessage: (message: IMessageDto) => void;
}

const MainLayoutContext =
    createContext<MainLayoutContextValues | undefined>(undefined);

export { MainLayoutContext, type MainLayoutContextValues };
