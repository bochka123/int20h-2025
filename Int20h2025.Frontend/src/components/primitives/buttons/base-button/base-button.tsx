/* eslint-disable no-unused-vars */
import { ButtonHTMLAttributes, FC, FormEvent, MouseEvent } from 'react';

import { ButtonLevelEnum, ButtonSizeEnum, useCommonButtonFunctions } from '../common';
// @ts-ignore
import styles from './base-button.module.scss';
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

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
    iconRight?: IconDefinition,
    classes?: string,
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
                                             iconRight,
                                             classes,
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
            ${enableHover ? styles.hover : ''}
            ${classes || ''}`}
            onClick={(e) => handleClick(e)}
            onMouseDown={handleFocus}
            onMouseUp={handleBlur}
            onMouseLeave={handleBlur}
            {...props}>

            <span className={styles.text}>
                {children}
                {iconRight && <FontAwesomeIcon icon={iconRight} />}
            </span>
        </button>
    );
};

export { BaseButton };
