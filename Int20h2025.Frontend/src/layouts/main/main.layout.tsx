import { FC } from 'react';

import styles from './main.layout.module.scss';

const MainLayout: FC = () => {
    return (
        <main className={styles.main}>
            <div className={styles.background} />
            <div className={styles.backgroundOverlay} />
            <div className={styles.mainContent}
                id={'mainContentContainer'}
            >

            </div>
        </main>
    );
};

export { MainLayout };
