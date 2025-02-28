/* eslint-disable no-unused-vars */
import { useContext } from 'react';

import { ToastModeEnum } from '@/common';
import { ToastContext } from '@/context';

interface ToastContextValues {
    addToast: (mode: ToastModeEnum, title: string, message?: string , delay?: number) => void,
}
const useToast = (): ToastContextValues => {
    const context = useContext(ToastContext);

    if (!context) {
        throw new Error('useToast must be used within a ToastProvider');
    }
    return context;
};

export { useToast };
