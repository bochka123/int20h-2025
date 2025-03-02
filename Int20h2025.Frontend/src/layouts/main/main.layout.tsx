import { FC } from 'react';

import { HistorySection, MainSection, RecentSection } from './components';
import { MainLayoutProvider } from './context';
import styles from './main.layout.module.scss';

const MainLayout: FC = () => {
    return (
        <main className={styles.main}>
            <div className={styles.mainContent} id={'mainContentContainer'}>
                <MainLayoutProvider>
                    <HistorySection />
                    <MainSection />
                    <RecentSection />
                </MainLayoutProvider>
            </div>
        </main>
    );
};

export { MainLayout };
