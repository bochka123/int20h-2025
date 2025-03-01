import { FC, ReactNode, useState } from 'react';

import { ToastModeEnum } from '@/common';
import { ToastContext } from '@/context';
import { uuid } from '@/helpers';
import { ToastList } from '@/modules';
import { ToastType } from '@/modules/toast-list/components';

type ToastProviderProps = {
    children: ReactNode,
}
const ToastProvider: FC<ToastProviderProps> = ({ children }) => {
    const [toastsList, setToastsList] = useState<(ToastType)[]>([]);

    const removeToast = (id: string): void => {
        setToastsList(prevState => prevState.filter(t => t.id !== id));
    };

    const addToast = (
        mode: ToastModeEnum,
        title: string,
        message = '',
        delay = 3000,
    ): void => {
        const id = uuid();

        setToastsList((prevState) => {
            return [...prevState, {
                id,
                title,
                message,
                mode,
                delay
            }];
        });

        setTimeout(() => removeToast(id), delay);
    };

    return (
        <ToastContext.Provider value={{
            addToast
        }}>
            {children}

            {toastsList.length > 0 ? <ToastList toasts={toastsList} removeToast={removeToast}/> : <></>}
        </ToastContext.Provider>
    );
};

export { ToastProvider };
