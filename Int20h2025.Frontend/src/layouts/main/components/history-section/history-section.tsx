import { FC } from 'react';

import styles from './history-section.module.scss';

type HistorySectionProps = {}
const HistorySection: FC<HistorySectionProps> = () => {
    return (
        <div className={styles.historySectionWrapper}>
            History
        </div>
    );
};

export { HistorySection };
