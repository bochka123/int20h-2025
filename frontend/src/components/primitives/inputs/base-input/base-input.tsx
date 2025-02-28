/* eslint-disable no-unused-vars */
import { ChangeEvent, FC, InputHTMLAttributes } from 'react';

import { InputTypes } from '@/common';

import styles from './base-input.module.scss';

type BaseInputProps = {
    value: string | number | undefined;
    onChange: (value: string | number | undefined) => void;
    labelText?: string;
    placeholder?: string;
    type?: InputTypes;
} & InputHTMLAttributes<HTMLInputElement>
const BaseInput: FC<BaseInputProps> = ({ value, onChange, labelText, placeholder, type = InputTypes.TEXT, ...props }) => {

    const handleChange = (e: ChangeEvent<HTMLInputElement>): void => {
        onChange && onChange(e.target.value);
    };

    return (
        <div>
            { labelText && <span className={styles.label}>{labelText}</span>}
            <input
                className={styles.button}
                value={value}
                onChange={handleChange}
                placeholder={placeholder}
                type={type}
                {...props}
            />
        </div>
    );
};

export { BaseInput };
