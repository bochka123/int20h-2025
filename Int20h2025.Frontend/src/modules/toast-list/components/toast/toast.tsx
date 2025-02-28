/* eslint-disable no-unused-vars */
import { faCheck, faCircleExclamation, faCircleInfo, faTriangleExclamation, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { FC } from 'react';

import { ToastModeEnum } from '@/common';

import styles from './toast.module.scss';
import { ToastType } from './toast.types.ts';

type ToastProps = {
    toast: ToastType
    removeToast: (id: string) => void
}
const Toast: FC<ToastProps> = ({ toast, removeToast }) => {

    const modeToIcon = {
        [ToastModeEnum.INFO]: faCircleInfo,
        [ToastModeEnum.SUCCESS]: faCheck,
        [ToastModeEnum.WARNING]: faCircleExclamation,
        [ToastModeEnum.ERROR]: faTriangleExclamation
    };

    return (
        <div
            className={`${styles.toast} 
            ${styles[toast.mode]}
            ${styles['small']}`}
        >
            <div className={styles.icon}>
                <FontAwesomeIcon icon={modeToIcon[toast.mode]} />
            </div>

            <div className={styles.toastInfo}>
                <h6 className={ styles['small']}>
                    {toast.title}
                </h6>
            </div>

            <div className={styles.closeIcon} onClick={() => removeToast(toast.id)}>
                <FontAwesomeIcon icon={faXmark} />
            </div>

        </div>
    );
};

export { Toast };
