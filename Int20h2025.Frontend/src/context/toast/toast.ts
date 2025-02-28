/* eslint-disable no-unused-vars */
import { createContext } from 'react';

import { ToastModeEnum } from '@/common';

interface ToastContextValues {
    addToast: (mode: ToastModeEnum, title: string, message?: string , delay?: number) => void,
}
const ToastContext = createContext<ToastContextValues | undefined>(undefined);

export { ToastContext };
