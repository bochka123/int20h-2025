import { faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { FC, ReactNode, useEffect, useRef } from 'react';
import { createPortal } from 'react-dom';

import styles from './modal.module.scss';
import { ModalProps } from './types';

const ESCAPE_KEY = 'Escape';

const Modal: FC<ModalProps> = ({ visible,
                                   setVisible,
                                   heading,
                                   zIndexType,
                                   children }) => {

    const wrapperRef = useRef<HTMLDivElement | null>(null);

    useEffect(() => {
        const close = (e: KeyboardEvent): void => {
            if (e.key === ESCAPE_KEY) {
                setVisible(false);
            }
        };
        window.addEventListener('keydown', close);
        return () => window.removeEventListener('keydown', close);
    }, []);

    const renderPortalBody = (): ReactNode => (
        <div className={`${styles.modalWrapper} ${styles[zIndexType || 'bottom']}`} ref={wrapperRef}>
            <div className={styles.modalContent}>
                <div className={styles.container}>
                    <div className={styles.heading}>
                        <p>{heading}</p>
                        <div className={styles.close} onClick={() => setVisible(false)}>
                            <FontAwesomeIcon icon={faXmark} />
                        </div>
                    </div>
                    {children}
                </div>
            </div>
        </div>
    );

    const renderPortal = (): ReactNode => {
        const modalLayer = document.getElementById('modal');

        if (modalLayer) {
            return createPortal(renderPortalBody(), modalLayer);
        }

        return null;
    };

    return (
        <>
            {visible && renderPortal()}
        </>
    );
};

export { Modal };
