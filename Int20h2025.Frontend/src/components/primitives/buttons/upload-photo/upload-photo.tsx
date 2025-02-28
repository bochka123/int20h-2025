import { faUpload } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { ChangeEvent, Dispatch, FC, SetStateAction } from 'react';

import styles from './upload-photo.module.scss';

type UploadPhotoProps = {
    setImageUrl: Dispatch<SetStateAction<string | undefined>>;
    setFile: Dispatch<SetStateAction<File | null>>;
}
const UploadPhoto: FC<UploadPhotoProps> = ({ setImageUrl, setFile }) => {

    const fileSelected = (event: ChangeEvent<HTMLInputElement>): void => {
        // @ts-ignore
        const file = event.target.files[0];

        if (file) {
            setImageUrl(URL.createObjectURL(file));
            setFile(file);
        }
    };

    return (
        <div>
            <label htmlFor={'fileInput'} className={styles.fileInputLabelWrapper}>
                <div className={styles.fileInputLabel}>
                    <FontAwesomeIcon icon={faUpload}/>
                </div>
            </label>
            <input id={'fileInput'} type="file" accept="image/*" onChange={fileSelected} className={styles.fileInput}/>
        </div>
    );
};

export { UploadPhoto };
