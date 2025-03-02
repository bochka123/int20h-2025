import { useRef,useState } from 'react';

import { ToastModeEnum } from '@/common';
import { useToast } from '@/hooks';

const grammar = '#JSGF V1.0; grammar words; public <word> = ' +
    'trello | azure | azure devops | board | add task | delete task | update task;';

type UseSpeechRecognitionReturnType = {
    message: string;
    isListening: boolean;
    initializeRecognition: () => void;
    startRecognition: () => void;
    stopRecognition: () => void;
    isSupported: boolean;
}
const useSpeechRecognition = (): UseSpeechRecognitionReturnType => {
    const [message, setMessage] = useState<string>('');
    const [isListening, setListening] = useState(false);
    
    const { addToast } = useToast();

    const isSupported = 'SpeechRecognition' in window || 'webkitSpeechRecognition' in window;

    // eslint-disable-next-line no-undef
    const recognitionRef = useRef<SpeechRecognition | null>(null);

    const initializeRecognition = (): void => {

        if (!isSupported) {
            addToast(ToastModeEnum.ERROR, 'Speech recognition is not supported');
            return;
        }

        recognitionRef.current = new (window.SpeechRecognition || window.webkitSpeechRecognition)();

        recognitionRef.current.lang = 'en-US';
        recognitionRef.current.interimResults = true;
        recognitionRef.current.maxAlternatives = 1;

        const speechRecognitionList = new (window.SpeechGrammarList || window.webkitSpeechGrammarList)();
        speechRecognitionList.addFromString(grammar, 1);
        recognitionRef.current.grammars = speechRecognitionList;

        recognitionRef.current.onresult = (event) => {
            let transcript = '';

            for (let i = event.resultIndex; i < event.results.length; i++) {
                const result = event.results[i];
                transcript += result[0].transcript;

                if (result.isFinal) {
                    transcript += ' ';
                }
            }

            setMessage(transcript);
        };

        recognitionRef.current.onend = () => {
            setListening(false);
            recognitionRef.current = null;
        };

        recognitionRef.current.onerror = (event) => {
            addToast(ToastModeEnum.ERROR, `Speech recognition error: ${event.error}`);
            setListening(false);
            recognitionRef.current = null;
        };
    };

    const startRecognition = (): void => {
        if (!isSupported) {
            addToast(ToastModeEnum.ERROR, 'Speech recognition is not supported');
            return;
        }

        if (!recognitionRef.current) {
            initializeRecognition();
        }

        recognitionRef.current?.start();
        setListening(true);
    };

    const stopRecognition = (): void => {
        if (!isSupported) {
            addToast(ToastModeEnum.ERROR, 'Speech recognition is not supported');
            return;
        }

        recognitionRef.current?.stop();
        setListening(false);
    };

    return {
        message,
        isListening,
        initializeRecognition,
        startRecognition,
        stopRecognition,
        isSupported: true,
    };
};

export { useSpeechRecognition };
