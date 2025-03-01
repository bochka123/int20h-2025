import { faArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FC, useState } from 'react';

import { IconButton, MultilineInput } from '@/components';
import { useProcessMutation } from '@/services/';

import styles from './main-section.module.scss';

type MainSectionProps = {}
const MainSection: FC<MainSectionProps> = () => {

    const [process] = useProcessMutation();
    const [isButtonDisabled, setButtonDisabled] = useState(false);

    const [message, setMessage] = useState<string>('');

    const handleSubmit = (): void => {
        setButtonDisabled(true);
        process({ prompt: message })
            .then((res) => {
                setButtonDisabled(false);
                console.log(res);
            })
            .catch((err) => {
                setButtonDisabled(false);
                console.error(err);
            });
    };
    
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
                        <IconButton
                            classes={styles.sendButton}
                            icon={faArrowUp}
                            onClick={handleSubmit}
                            disabled={!message || isButtonDisabled}
                            isLoading={isButtonDisabled}
                        />
                    </div>
                </div>
            </div>
        </div>
    );
};

export { MainSection };
