import { faTrello } from '@fortawesome/free-brands-svg-icons';
import {
    faArrowRightFromBracket,
    faArrowUp,
    faMicrophone,
    faMicrophoneSlash,
} from '@fortawesome/free-solid-svg-icons';
import { FC, KeyboardEventHandler, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import { BaseButton, IconButton, MultilineInput } from '@/components';
import { useSpeechRecognition } from '@/hooks';
import { useMainLayoutContext } from '@/layouts/main/hooks';
import { useLogOutMutation, useProcessMutation } from '@/services';

import styles from './main-section.module.scss';
import { SyncTrelloModal } from '@/modules';

type MainSectionProps = {}
const MainSection: FC<MainSectionProps> = () => {

    const { addMessage } = useMainLayoutContext();

    const {
        message: speechMessage,
        isSupported,
        isListening,
        startRecognition,
        stopRecognition
    } = useSpeechRecognition();

    const [process] = useProcessMutation();
    const [logOut] = useLogOutMutation();
    const [isButtonDisabled, setButtonDisabled] = useState(false);

    const navigate = useNavigate();

    const [message, setMessage] = useState<string>('');
    const [syncTrelloModalVisible, setSyncTrelloModalVisible] = useState(false);
    
    useEffect(() => {
        if (isSupported && isListening) {
            setMessage(speechMessage);
        }
    }, [speechMessage]);

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
                <BaseButton
                    iconRight={faTrello}
                    onClick={() => setSyncTrelloModalVisible(true)}
                    classes={styles.trelloButton}
                >
                    Sync with
                </BaseButton>
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
                <IconButton
                    icon={!isListening ? faMicrophone : faMicrophoneSlash}
                    onClick={!isListening ? startRecognition : stopRecognition}
                    disabled={!isSupported}
                />
                <IconButton
                    icon={faArrowRightFromBracket}
                    onClick={handleLogOut}
                />
            </div>

            <SyncTrelloModal
                visible={syncTrelloModalVisible}
                setVisible={setSyncTrelloModalVisible}
            />
        </div>
    );
};

export { MainSection };
