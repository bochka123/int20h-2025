import { faSpinner } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { FC } from 'react';

import styles from './loader.module.scss';

type LoaderProps = {
    size?: number;
}

const Loader: FC<LoaderProps> = ({ size = 100 }) => {
    return (
        <div className={styles.loaderWrapper}>
            <FontAwesomeIcon
                icon={faSpinner}
                className={styles.loaderImage}
                style={{ width: size, height: size }}
            />
        </div>
    );
};

export { Loader };
