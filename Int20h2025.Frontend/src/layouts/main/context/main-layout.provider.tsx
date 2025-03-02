import { FC, ReactNode, useState } from 'react';

import { IMessageDto } from '@/models/responses';

import { MainLayoutContext } from './main-layout.context.ts';

type MainLayoutProviderProps = {
    children: ReactNode;
}
const MainLayoutProvider: FC<MainLayoutProviderProps> = ({ children }) => {

    const [recentMessages, setRecentMessages] = useState<IMessageDto[]>([]);

    const addMessage = (message: IMessageDto): void => {
        setRecentMessages((prevMessages) => {
            const messages = [message, ...prevMessages];
            if (messages.length > 20) return messages.slice(0, 20);
            return messages;
        });
    };

    return (
        <MainLayoutContext.Provider value={
            {
                recentMessages,
                addMessage,
            }
        }>
            {children}
        </MainLayoutContext.Provider>
    );
};

export { MainLayoutProvider };
