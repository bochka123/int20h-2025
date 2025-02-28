/* eslint-disable no-unused-vars */
import { FC, ReactNode } from 'react';
import { createPortal } from 'react-dom';

import { Toast, ToastType } from './components';
import styles from './toast-list.module.scss';

type ToastListProps = {
    toasts: (ToastType)[],
    removeToast: (id: string) => void,
}
const ToastList: FC<ToastListProps> = ({
                                           toasts,
                                           removeToast,
                                       }) => {

    const renderPortalBody = (): ReactNode => {
        return (
            <div className={styles.toastList}>
                {
                    toasts.map(toast => {
                        return <Toast toast={toast} key={`toast-${toast.id}`} removeToast={removeToast}/>;
                    })
                }
            </div>
        );
    };
    const renderPortal = (): ReactNode => {
        const modalLayer = document.querySelector('#toast');

        if (modalLayer) {
            return createPortal(renderPortalBody(), modalLayer);
        }

        return null;
    };

    return (
        <>
            {renderPortal()}
        </>
    );
};

export { ToastList };
