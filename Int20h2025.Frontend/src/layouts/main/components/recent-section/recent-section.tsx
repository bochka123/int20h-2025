import { FC } from 'react';

import { Message } from '@/components';
import { useMainLayoutContext } from '@/layouts/main/hooks';

import styles from './recent-section.module.scss';

type RecentSectionProps = {}
const RecentSection: FC<RecentSectionProps> = () => {

    const { recentMessages } = useMainLayoutContext();

    return (
        <div className={styles.recentSectionWrapper}>
            <p>Recent</p>
            <div className={styles.messagesContainer}>
                {
                    recentMessages.map((message, index) => (
                        <Message key={`recent-message-${index}`}>
                            {message.message}
                        </Message>
                    ))
                }
            </div>
        </div>
    );
};

export { RecentSection };
