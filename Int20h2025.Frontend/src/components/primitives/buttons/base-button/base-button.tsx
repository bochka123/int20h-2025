/* eslint-disable no-unused-vars */
import { ButtonHTMLAttributes, FC, FormEvent, MouseEvent } from 'react';

import { ButtonLevelEnum, ButtonSizeEnum, useCommonButtonFunctions } from '../common';
// @ts-ignore
import styles from './base-button.module.scss';

type BaseButtonProps = {
    level?: ButtonLevelEnum,
    size?: ButtonSizeEnum,
    transparent?: boolean,
    onClick?: (
        e: MouseEvent<HTMLButtonElement> | FormEvent<HTMLFormElement>
    ) => void,
    buttonClasses?: string,
    isLoading?: boolean,
    enableHover?: boolean,
} & ButtonHTMLAttributes<HTMLButtonElement>;

const BaseButton: FC<BaseButtonProps> = ({
                                             level = ButtonLevelEnum.PRIMARY_BLUE,
                                             size = ButtonSizeEnum.MEDIUM,
                                             transparent = false,
                                             children,
                                             onClick,
                                             buttonClasses,
                                             isLoading,
                                             enableHover = true,
                                             ...props
                                         }) => {
    const { isFocused, handleFocus, handleBlur, handleClick } = useCommonButtonFunctions(props, onClick);
    return (
        <button
            type={props.type ? props.type : 'button'}
            className={`${styles.button} 
            ${styles[level]}
            ${styles[size]}
            ${isFocused ? styles['focused'] : ''}
            ${transparent ? styles['transparent'] : ''}
            ${buttonClasses || ''} 
            ${isLoading ? styles.loading : ''}
            ${enableHover ? styles.hover : ''}`}
            onClick={(e) => handleClick(e)}
            onMouseDown={handleFocus}
            onMouseUp={handleBlur}
            onMouseLeave={handleBlur}
            {...props}>

            <span className={styles.text}>
                {children}
            </span>
        </button>
    );
};

export { BaseButton };
