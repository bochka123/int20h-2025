import { faTrello } from '@fortawesome/free-brands-svg-icons';
import {
    faArrowRightFromBracket,
    faArrowUp,
    faMicrophone,
    faMicrophoneSlash,
} from '@fortawesome/free-solid-svg-icons';
import { FC, KeyboardEventHandler, useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';

import { IntegrationSystemEnum, ToastModeEnum } from '@/common';
import { BaseButton, IconButton, MultilineInput } from '@/components';
import { useSpeechRecognition, useToast } from '@/hooks';
import { useMainLayoutContext } from '@/layouts/main/hooks';
import { IApiResponseDto } from '@/models/responses';
import { SyncTrelloModal } from '@/modules';
import { useCheckIntegrationMutation, useLogOutMutation, useProcessMutation } from '@/services';
import { logOut } from '@/store/auth';

import styles from './main-section.module.scss';

type MainSectionProps = {}
const MainSection: FC<MainSectionProps> = () => {

    const { addMessage } = useMainLayoutContext();
    const { addToast } = useToast();
    const dispatch = useDispatch();

    const [trelloStatus, setTrelloStatus] = useState<IApiResponseDto<null>>();
    const [checkTrelloSuccess] = useCheckIntegrationMutation();

    useEffect(() => {
        if (!trelloStatus) {
            checkTrelloSuccess({ systemName: IntegrationSystemEnum.TRELLO })
                .unwrap()
                .then((data) => {
                    setTrelloStatus(data);
                }).catch((error) => {
                    setTrelloStatus({
                        ok: false,
                        message: error.message,
                        data: null,
                    });
                    addToast(ToastModeEnum.ERROR, 'Error checking Trello integration');
                });
        }
    }, []);

    const {
        message: speechMessage,
        isSupported,
        isListening,
        startRecognition,
        stopRecognition
    } = useSpeechRecognition();

    const [process] = useProcessMutation();
    const [logOutMutation] = useLogOutMutation();

    const { addHistory, messageFromHistory, setMessageFromHistory } = useMainLayoutContext();

    useEffect(() => {
        if (messageFromHistory) {
            setMessage(messageFromHistory);
            setMessageFromHistory(null);
        }
    }, [messageFromHistory]);

    const navigate = useNavigate();

    const [message, setMessage] = useState<string>('');
    const [syncTrelloModalVisible, setSyncTrelloModalVisible] = useState(false);
    const [isButtonDisabled, setButtonDisabled] = useState(false);
    
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
                    setMessage('');
                    if (res.data?.data.clarification) {
                        addMessage({ message: res.data?.data.clarification });
                        addHistory({ text: message, result: res.data.data.clarification, success: res.data.ok });
                    } else {
                        addToast(ToastModeEnum.ERROR, 'AI did not generate correct answer. Please try again');
                    }
                })
                .catch((err) => {
                    setButtonDisabled(false);
                    addToast(ToastModeEnum.ERROR, err.message);
                });
        }
    };

    const handleKeyDown: KeyboardEventHandler<HTMLTextAreaElement> = (event): void => {
        if (event.ctrlKey && event.key === 'Enter') {
            handleSubmit();
        }
    };

    const handleLogOut = (): void => {
        logOutMutation()
            .then(() => {
                dispatch(logOut());
                navigate('/auth');
            });
    };
    
    return (
        <div className={styles.mainSectionWrapper}>
            <div className={styles.header}>
                <BaseButton
                    iconRight={faTrello}
                    onClick={() => setSyncTrelloModalVisible(true)}
                    classes={styles.trelloButton}
                    isLoading={!trelloStatus}
                    disabled={!trelloStatus}
                >
                    {trelloStatus && trelloStatus.ok ? 'Synchronized' : 'Sync with'}
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
