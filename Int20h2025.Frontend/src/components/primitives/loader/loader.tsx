import { FC } from 'react';

import styles from './loader.module.scss';

type LoaderProps = {
    size?: number;
}

const Loader: FC<LoaderProps> = ({ size = 100 }) => {
    return (
        <div className={styles.loaderWrapper}>
            <img
                src={''}
                alt="Loading..."
                className={styles.loaderImage}
                style={{ width: size, height: size }}
            />
        </div>
    );
};

export { Loader };
