/* eslint-disable no-unused-vars */
import { createContext, Dispatch, SetStateAction } from 'react';

import { IHistoryItemDto, IMessageDto } from '@/models/responses';

interface MainLayoutContextValues {
    recentMessages: IMessageDto[];
    addMessage: (message: IMessageDto) => void;
    historyItems: IHistoryItemDto[];
    addHistory: (history: IHistoryItemDto) => void;
    messageFromHistory: string | null;
    setMessageFromHistory: Dispatch<SetStateAction<string | null>>;
}

const MainLayoutContext =
    createContext<MainLayoutContextValues | undefined>(undefined);

export { MainLayoutContext, type MainLayoutContextValues };
