import { faCheck, faComment,faQuestion, faRepeat, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { FC } from 'react';

import { IconButton, Loader, Message } from '@/components';
import { useMainLayoutContext } from '@/layouts/main/hooks';

import styles from './history-section.module.scss';

type HistorySectionProps = {
    isLoading: boolean;
}
const HistorySection: FC<HistorySectionProps> = ({ isLoading }) => {
    
    const { historyItems, setMessageFromHistory } = useMainLayoutContext();

    return (
        <div className={styles.historySectionWrapper}>
            <p>History</p>
            <div className={styles.messagesContainer}>
                {
                    isLoading ? <Loader /> :
                    historyItems?.map((history, index) => (
                        <Message key={`history-item-${index}`}>
                            <div className={styles.title}>Question <FontAwesomeIcon icon={faQuestion} /> :</div>
                            <div>{history.text}</div>
                            <div className={styles.space}/>
                            <div className={styles.title}>Answer <FontAwesomeIcon icon={faComment} /> :</div>
                            <div>{history.result}</div>
                            <div className={styles.status}>
                                <IconButton
                                    icon={faRepeat}
                                    onClick={() => setMessageFromHistory(history.text)}
                                />
                                <FontAwesomeIcon icon={history.success ? faCheck : faXmark} />
                            </div>
                        </Message>
                    ))
                }
            </div>
        </div>
    );
};

export { HistorySection };
