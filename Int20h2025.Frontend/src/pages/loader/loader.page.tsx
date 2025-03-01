import { FC } from 'react';

import { Loader } from '@/components';

import styles from './loader.page.module.scss';

type LoaderPageProps = {}
const LoaderPage: FC<LoaderPageProps> = () => {
    return (
        <div className={styles.loaderWrapper}>
            <Loader />
        </div>
    );
};

export { LoaderPage };
