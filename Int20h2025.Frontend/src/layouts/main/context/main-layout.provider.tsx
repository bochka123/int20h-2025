import { FC, ReactNode, useEffect, useState } from 'react';

import { IHistoryItemDto, IMessageDto } from '@/models/responses';

import { MainLayoutContext } from './main-layout.context.ts';

type MainLayoutProviderProps = {
    children: ReactNode;
    providerHistoryItems: IHistoryItemDto[];
}
const MainLayoutProvider: FC<MainLayoutProviderProps> = ({ children, providerHistoryItems }) => {

    const [recentMessages, setRecentMessages] = useState<IMessageDto[]>([]);
    const [historyItems, setHistoryItems] = useState<IHistoryItemDto[]>(providerHistoryItems);
    const [messageFromHistory, setMessageFromHistory] = useState<string | null>(null);

    useEffect(() => {
        setHistoryItems(providerHistoryItems);
    }, [providerHistoryItems]);

    const addMessage = (message: IMessageDto): void => {
        setRecentMessages((prevMessages) => {
            const messages = [message, ...prevMessages];
            if (messages.length > 20) return messages.slice(0, 20);
            return messages;
        });
    };

    const addHistory = (history: IHistoryItemDto): void => {
        setHistoryItems((prevItems) => {
            const items = [history, ...prevItems];
            if (items.length > 10) return items.slice(0, 10);
            return items;
        });
    };

    return (
        <MainLayoutContext.Provider value={
            {
                recentMessages,
                addMessage,
                historyItems,
                addHistory,
                messageFromHistory,
                setMessageFromHistory,
            }
        }>
            {children}
        </MainLayoutContext.Provider>
    );
};

export { MainLayoutProvider };
