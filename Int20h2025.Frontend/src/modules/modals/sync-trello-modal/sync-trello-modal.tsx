import { Dispatch, FC, SetStateAction } from 'react';
import { Controller, SubmitErrorHandler, SubmitHandler, useForm } from 'react-hook-form';

import { ToastModeEnum } from '@/common';
import { BaseButton, BaseInput, Modal } from '@/components';
import { getFormErrorMessage } from '@/helpers';
import { useToast } from '@/hooks';
import styles from '@/pages/auth/auth-page.module.scss';

type FormNames = {
    apiKey: string;
    token: string;
}

type SyncTrelloModalProps = {
    visible: boolean;
    setVisible: Dispatch<SetStateAction<boolean>>;
}
const SyncTrelloModal: FC<SyncTrelloModalProps> = ({ visible, setVisible }) => {

    const { addToast } = useToast();

    const { handleSubmit, control } = useForm<FormNames>();

    const onSubmit: SubmitHandler<FormNames> = (data): void => {
        console.log(data);
    };

    const onError: SubmitErrorHandler<FormNames> = (error): void => {
        addToast(ToastModeEnum.ERROR, getFormErrorMessage(error));
    };

    return (
        <Modal heading={'Synchronize with Trello'} visible={visible} setVisible={setVisible}>
            <form onSubmit={handleSubmit(onSubmit, onError)} className={styles.form}>
                <Controller
                    control={control}
                    name="apiKey"
                    rules={{ required: 'Api key field is required' }}
                    render={({ field: { onChange, value } }) => (
                        <BaseInput value={value} onChange={onChange} placeholder={'Api key'} />
                    )}
                />
                <Controller
                    control={control}
                    name="token"
                    rules={{ required: 'Token field is required' }}
                    render={({ field: { onChange, value } }) => (
                        <BaseInput value={value} onChange={onChange} placeholder={'Token'} />
                    )}
                />

                <BaseButton type={'submit'}>Sync</BaseButton>
            </form>
        </Modal>
    );
};

export { SyncTrelloModal };
