import { FC, ReactNode, useEffect, useRef } from 'react';

import styles from './message.module.scss';

type MessageProps = {
    children: ReactNode;
    isHighlighted?: boolean;
}
const Message: FC<MessageProps> = ({ children, isHighlighted = false, }) => {

    const messageRef = useRef<HTMLDivElement>(null);

    useEffect(() => {
        if (!messageRef.current) return;

        if (isHighlighted) {
            messageRef.current.classList.add(styles.highlighted);

            const timeout = setTimeout(() => {
                messageRef.current?.classList.remove(styles.highlighted);
            }, 1000);

            return () => {
                clearTimeout(timeout);
            };
        }
    }, [isHighlighted, children]);

    return (
        <div ref={messageRef} className={styles.messageWrapper}>
            {children}
        </div>
    );
};

export { Message };
