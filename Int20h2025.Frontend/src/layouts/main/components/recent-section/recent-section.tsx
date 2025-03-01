import { FC } from 'react';

import styles from './recent-section.module.scss';

type RecentSectionProps = {}
const RecentSection: FC<RecentSectionProps> = () => {
    return (
        <div className={styles.recentSectionWrapper}>
            Recent
        </div>
    );
};

export { RecentSection };
