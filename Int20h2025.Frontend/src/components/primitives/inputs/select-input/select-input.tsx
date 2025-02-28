import { ChangeEventHandler, FC, useId } from 'react';

import styles from './select-input.module.scss';

type SelectInputProps = {
    value: string;
    onChange: ChangeEventHandler<HTMLSelectElement>;
    options: string[];
    labelText?: string;
}
const SelectInput: FC<SelectInputProps> = ({ value, onChange, options, labelText }) => {
    const id = useId();

    return (
        <div className={styles.selectWrapper}>
            { labelText && <span className={styles.label}>{labelText}</span>}
            <select value={value} onChange={onChange} className={styles.select}>
                {
                    ['Not selected', ...options].map((option, index) => (
                        <option key={`select-option-${id}-${index}`} value={option} defaultValue={undefined}>
                            {option}
                        </option>
                    ))
                }
            </select>
        </div>
    );
};

export { SelectInput };
