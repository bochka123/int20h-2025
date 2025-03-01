/* eslint-disable no-unused-vars */
import { ChangeEvent, FC, HTMLProps, useRef } from 'react';

import styles from './multiline-input.module.scss';

type MultilineInputType = {
    value: string,
    onChange?: (value: string) => void,
    labelText?: string,
    extraInfo?: string,
    dark?: boolean,
    maxLength?: number,
    rows?: number,
    cols?: number,
};

type TextAreaHTMLProps = HTMLProps<HTMLTextAreaElement>

type MultilineInputProps = Omit<TextAreaHTMLProps, keyof MultilineInputType> & MultilineInputType;

const MultilineInput: FC<MultilineInputProps> = ({
    value = '',
    onChange,
    labelText,
    extraInfo,
    dark,
    maxLength,
    rows = 1,
    cols = 1,
    ...props
}) => {

    const textareaRef = useRef<HTMLTextAreaElement>(null);

    const autoResize = (): void => {
        if (!textareaRef.current) return;
        textareaRef.current.style.height = 'auto';
        textareaRef.current.style.height = textareaRef.current.scrollHeight < 300
            ? `${textareaRef.current.scrollHeight}px`
            : '300px';
    };

    const handleChange = (e: ChangeEvent<HTMLTextAreaElement>): void => {
        onChange && onChange(e.target.value);
        autoResize();
    };

    return (
        <div>
            { labelText && <span className={styles.label}>{labelText}</span>}
                <div className={styles.inputContainer}>
                    <textarea
                        ref={textareaRef}
                        value={value}
                        onChange={handleChange}
                        className={styles.input}
                        maxLength={maxLength}
                        rows={rows}
                        cols={cols}
                        {...props}
                    />

                    {
                        !!maxLength &&
                        <p
                            className={styles.maxLength}
                        >
                            {value?.length || '0'}/{maxLength}
                        </p>
                    }
                </div>
        </div>
    );
};

export { MultilineInput };
