import { faGripLinesVertical } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { FC, useRef } from 'react';

import { HistorySection, MainSection, RecentSection } from './components';
import { MainLayoutProvider } from './context';
import styles from './main.layout.module.scss';
import { useResizeMainLayout } from '@/layouts/main/hooks';

const MainLayout: FC = () => {

    const mainContentRef = useRef<HTMLDivElement>(null);
    const leftResizeLineRef = useRef<HTMLDivElement>(null);
    const rightResizeLineRef = useRef<HTMLDivElement>(null);

    useResizeMainLayout(mainContentRef, leftResizeLineRef, rightResizeLineRef);
    
    return (
        <main className={styles.main}>
            <div className={styles.mainContent} ref={mainContentRef} id={'mainContentContainer'}>
                <MainLayoutProvider>
                    <HistorySection />
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
