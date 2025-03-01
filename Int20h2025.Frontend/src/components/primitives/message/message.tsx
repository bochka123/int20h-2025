import { FC, ReactNode } from 'react';

import styles from './message.module.scss';

type MessageProps = {
    children: ReactNode;
}
const Message: FC<MessageProps> = ({ children }) => {
    return (
        <div className={styles.messageWrapper}>
            {children}
        </div>
    );
};

export { Message };
