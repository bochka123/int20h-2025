import { faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import { FC, useState } from 'react';

import { IconButton, MultilineInput } from '@/components';

import styles from './main-section.module.scss';

type MainSectionProps = {}
const MainSection: FC<MainSectionProps> = () => {

    const [message, setMessage] = useState<string>('');

    return (
        <div className={styles.mainSectionWrapper}>
            <div className={styles.inputWrapper}>
                <MultilineInput value={message} onChange={setMessage} rows={2} />
                <IconButton icon={faPaperPlane} />
            </div>
        </div>
    );
};

export { MainSection };
