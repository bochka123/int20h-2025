/* eslint-disable no-unused-vars */
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { ButtonHTMLAttributes, FC, FormEvent, MouseEvent, useState } from 'react';

import { ButtonSizeEnum, useCommonButtonFunctions } from '../common';
import styles from './icon-button.module.scss';

type IconButtonProps = {
    icon: IconDefinition;
    color?: 'primary' | 'red' | 'orange' | 'green' | 'gray' | 'dark-gray',
    hoverColor?: 'primary' | 'red' | 'orange' | 'green' | 'gray' | 'dark-gray',
    size?: ButtonSizeEnum,
    outline?: boolean,
    transparent?: boolean,
    onClick?: (
        e: MouseEvent<HTMLButtonElement> | FormEvent<HTMLFormElement>
    ) => void,
    disabled?: boolean,
    classes?: string,
} & ButtonHTMLAttributes<HTMLButtonElement>;

const IconButton: FC<IconButtonProps> = ({
    icon,
    color = 'primary',
    hoverColor = color === 'primary' ? 'primary' : 'red',
    size = ButtonSizeEnum.MEDIUM,
    outline = true,
    transparent = false,
    onClick,
    disabled,
    classes,
    ...props
}) => {

    const [_, setIsHovered] = useState<boolean>(false);

    // eslint-disable-next-line @typescript-eslint/no-unsafe-call,@typescript-eslint/no-unsafe-assignment
    const { isFocused, handleFocus, handleBlur, handleClick } = useCommonButtonFunctions(props, onClick);

    const handleHover = (): void => {
        setIsHovered(true);
    };

    const handleLeave = (): void => {
        setIsHovered(false);
    };

    return (
        <button
            type={props.type ? props.type : 'button'}
            className={`
                ${styles.button}
                ${styles[color]}
                ${styles[size]}
                ${styles[`hover-${hoverColor}`]}
                ${styles[`outline-${String(outline)}`]}
                ${isFocused ? styles['focused'] : ''}
                ${transparent ? styles['transparent'] : ''}
                ${disabled ? styles.disabled : ''}
                ${classes}
            `}
            onClick={(e) => handleClick(e)}
            onMouseDown={handleFocus}
            onMouseUp={handleBlur}
            onMouseLeave={() => {
                handleBlur();
                handleLeave();
            }}
            onMouseMove={handleHover}
            {...props}
        >
            <FontAwesomeIcon icon={icon} />
        </button>
    );
};

export { IconButton };
