import React, { ReactNode } from 'react';

type ModalProps = {
    visible: boolean;
    setVisible: React.Dispatch<React.SetStateAction<boolean>>;
    heading: string;
    zIndexType?: 'top' | 'bottom';
    children: ReactNode;
}

export { type ModalProps };
