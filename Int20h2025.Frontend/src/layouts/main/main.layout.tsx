import { FC } from 'react';

import { HistorySection, MainSection, RecentSection } from './components';
import styles from './main.layout.module.scss';

const MainLayout: FC = () => {
    return (
        <main className={styles.main}>
            <div className={styles.mainContent}
                id={'mainContentContainer'}
            >
                <HistorySection />
                <MainSection />
                <RecentSection />
            </div>
        </main>
    );
};

export { MainLayout };
