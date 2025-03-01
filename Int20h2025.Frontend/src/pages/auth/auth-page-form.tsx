import { FC } from 'react';
import { Controller, SubmitErrorHandler, SubmitHandler, useForm } from 'react-hook-form';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';

import { InputTypes, ToastModeEnum } from '@/common';
import { BaseButton, BaseInput } from '@/components';
import { getFormErrorMessage } from '@/helpers';
import { useToast } from '@/hooks';
import { IAuthRequestDto } from '@/models/requests';
import {  useLoginMutation, useRegisterMutation } from '@/services';
import { setProfile } from '@/store/auth';

import styles from './auth-page.module.scss';
import { GoogleAuthButton } from './components';

type FormNames = {
    email: string;
    password: string;
}

type AuthPageFormProps = {
    authType: 'signIn' | 'signUp';
}

const AuthPageForm: FC<AuthPageFormProps> = ({ authType }) => {
    const [logIn] = useLoginMutation();
    const [signUp] = useRegisterMutation();
    const { addToast } = useToast();
    const dispatch = useDispatch();
    const navigate = useNavigate();

    const { handleSubmit, control } = useForm<FormNames>();

    const onSubmit: SubmitHandler<FormNames> = (data): void => {
        const requestData: IAuthRequestDto = {
            email: data.email,
            password: data.password,
        };

        authType === 'signIn'
            ? logIn(requestData)
                .unwrap()
                .then((res) => {
                    dispatch(setProfile({ id: res.data.id }));
                    navigate('/');
                })
                .catch(() => addToast(ToastModeEnum.ERROR, 'Failed to log in'))
            : signUp(requestData)
                .unwrap()
                .then((res) => {
                    dispatch(setProfile({ id: res.data.id }));
                    navigate('/');
                })
                .catch(() => addToast(ToastModeEnum.ERROR, 'Failed to register'));
    };

    const onError: SubmitErrorHandler<FormNames> = (error): void => {        
        addToast(ToastModeEnum.ERROR, getFormErrorMessage(error));
    };

    return (
        <form onSubmit={handleSubmit(onSubmit, onError)} className={styles.form}>
            <Controller
                control={control}
                name="email"
                rules={{
                    required: 'Email field is required',
                    pattern: {
                        value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
                        message: 'Invalid email format',
                    }
                }}
                render={({ field: { onChange, value } }) => (
                    <BaseInput value={value} onChange={onChange} placeholder={'Email'} type={InputTypes.TEXT} />
                )}
            />
            <Controller
                control={control}
                name="password"
                rules={{ required: 'Password field is required' }}
                render={({ field: { onChange, value } }) => (
                    <BaseInput value={value} onChange={onChange} placeholder={'Password'} type={InputTypes.PASSWORD} />
                )}
            />

            <BaseButton type={'submit'}>
                {authType === 'signIn' ? 'Login' : 'Register'}
            </BaseButton>
            <GoogleAuthButton />
        </form>
    );
};

export { AuthPageForm };
