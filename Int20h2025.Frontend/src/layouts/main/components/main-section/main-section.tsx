import { faArrowRightFromBracket, faArrowUp } from '@fortawesome/free-solid-svg-icons';
import { FC, KeyboardEventHandler, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import { IconButton, MultilineInput } from '@/components';
import { useLogOutMutation, useProcessMutation } from '@/services';

import styles from './main-section.module.scss';
import { useMainLayoutContext } from '@/layouts/main/hooks';

type MainSectionProps = {}
const MainSection: FC<MainSectionProps> = () => {

    const { addMessage } = useMainLayoutContext();

    const [process] = useProcessMutation();
    const [logOut] = useLogOutMutation();
    const [isButtonDisabled, setButtonDisabled] = useState(false);

    const navigate = useNavigate();

    const [message, setMessage] = useState<string>('');

    const handleSubmit = (): void => {
        if (message) {
            setButtonDisabled(true);
            process({ prompt: message })
                .then((res) => {
                    setButtonDisabled(false);
                    if (res.data) {
                        addMessage({ message: res.data.data.clarification });
                    }
                })
                .catch((err) => {
                    setButtonDisabled(false);
                    console.error(err);
                });
        }
    };

    const handleKeyDown: KeyboardEventHandler<HTMLTextAreaElement> = (event): void => {
        if (event.ctrlKey && event.key === 'Enter') {
            handleSubmit();
        }
    };

    const handleLogOut = (): void => {
        logOut();
        navigate('/auth');
    };
    
    return (
        <div className={styles.mainSectionWrapper}>
            <div className={styles.header}>

            </div>
            <div className={styles.generalWrapper}>
                <h1>Bobr intelligence</h1>
                <div className={styles.inputWrapper}>
                    <MultilineInput
                        placeholder={'Enter your prompt...'}
                        value={message}
                        onChange={setMessage}
                        rows={2}
                        classes={styles.textarea}
                        onKeyDown={handleKeyDown}
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
            <div className={styles.footer}>
                <IconButton icon={faArrowRightFromBracket} onClick={handleLogOut} />
            </div>
        </div>
    );
};

export { MainSection };
