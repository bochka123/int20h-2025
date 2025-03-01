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
                    recentMessages[0] &&
                    <Message
                        isHighlighted={true}
                        key={'recent-message'}
                    >
                        {recentMessages[0].message}
                    </Message>
                }
                {
                    recentMessages.map((message, index) => (
                        index !== 0 &&
                        <Message
                            key={`recent-message-${index}`}
                        >
                            {message.message}
                        </Message>
                    ))
                }
            </div>
        </div>
    );
};

export { RecentSection };
