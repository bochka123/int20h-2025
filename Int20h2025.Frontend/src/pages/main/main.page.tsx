import { FC } from 'react';
import { Outlet } from 'react-router-dom';

import styles from './main.page.module.scss';

const MainPage: FC = () => {
    return (
        <div className={styles.background}>
            <Outlet />
        </div>
    );
};

export { MainPage };
