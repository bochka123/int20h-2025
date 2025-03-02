import { faGripLinesVertical } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { FC, useRef } from 'react';

import { useResizeMainLayout } from '@/layouts/main/hooks';

import { HistorySection, MainSection, RecentSection } from './components';
import { MainLayoutProvider } from './context';
import styles from './main.layout.module.scss';
import { useGetHistoryQuery } from '@/services';

const MainLayout: FC = () => {

    const mainContentRef = useRef<HTMLDivElement>(null);
    const leftResizeLineRef = useRef<HTMLDivElement>(null);
    const rightResizeLineRef = useRef<HTMLDivElement>(null);

    useResizeMainLayout(mainContentRef, leftResizeLineRef, rightResizeLineRef);

    const { data: historyData, isLoading: isHistoryLoading } = useGetHistoryQuery();
    
    return (
        <main className={styles.main}>
            <div className={styles.mainContent} ref={mainContentRef} id={'mainContentContainer'}>
                <MainLayoutProvider providerHistoryItems={historyData?.data ?? []}>
                    <HistorySection isLoading={isHistoryLoading}/>
                    <div className={styles.lineWrapper} ref={leftResizeLineRef}>
                        <FontAwesomeIcon icon={faGripLinesVertical} />
                    </div>
                    <MainSection />
                    <div className={styles.lineWrapper} ref={rightResizeLineRef}>
                        <FontAwesomeIcon icon={faGripLinesVertical} />
                    </div>
                    <RecentSection />
                </MainLayoutProvider>
            </div>
        </main>
    );
};

export { MainLayout };
