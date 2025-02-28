/* eslint-disable no-unused-vars */
import { ButtonHTMLAttributes, FormEvent, MouseEvent, useState } from 'react';

type PropsType = {
  onClick?: (
    e: MouseEvent<HTMLButtonElement> | FormEvent<HTMLFormElement>
  ) => void;
} & ButtonHTMLAttributes<HTMLButtonElement>;

type CommonButtonFunctionsType = {
  isFocused: boolean;
  handleFocus: () => void;
  handleBlur: () => void;
  handleClick: (e: MouseEvent<HTMLButtonElement> | FormEvent<HTMLFormElement>) => void;
};

const useCommonButtonFunctions = <T extends PropsType>(props: T, onClick?: T['onClick']): CommonButtonFunctionsType => {
  const [isFocused, setIsFocused] = useState<boolean>(false);

  const handleFocus = (): void => {
    setIsFocused(true);
  };

  const handleBlur = (): void => {
    setIsFocused(false);
  };

  const handleClick = (e: MouseEvent<HTMLButtonElement> | FormEvent<HTMLFormElement>): void => {
    if (props.type !== 'submit') {
      e.preventDefault();
    }

    onClick && onClick(e);
  };

  return {
    isFocused,
    handleFocus,
    handleBlur,
    handleClick,
  };
};

export { useCommonButtonFunctions };
