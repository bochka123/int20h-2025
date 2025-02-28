import { FC } from 'react';
import { Outlet } from 'react-router-dom';

import styles from './main.layout.module.scss';

const MainLayout: FC = () => {
    return (
        <main className={styles.main}>
            <div className={styles.background} />
            <div className={styles.backgroundOverlay} />
            <div className={styles.mainContent}
                id={'mainContentContainer'}
            >
                <Outlet />
            </div>
        </main>
    );
};

export { MainLayout };
