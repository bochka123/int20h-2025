import { faArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FC, useState } from 'react';

import { IconButton, MultilineInput } from '@/components';

import styles from './main-section.module.scss';

type MainSectionProps = {}
const MainSection: FC<MainSectionProps> = () => {

    const [message, setMessage] = useState<string>('');

    return (
        <div className={styles.mainSectionWrapper}>
            <div className={styles.generalWrapper}>
                <h1>Bobr intelligence</h1>
                <div className={styles.inputWrapper}>
                    <MultilineInput
                        placeholder={'Enter your prompt...'}
                        value={message}
                        onChange={setMessage}
                        rows={2}
                        classes={styles.textarea}
                    />
                    <div className={styles.sendButtonWrapper}>
                        <IconButton classes={styles.sendButton} icon={faArrowUp} />
                    </div>
                </div>
            </div>
        </div>
    );
};

export { MainSection };
