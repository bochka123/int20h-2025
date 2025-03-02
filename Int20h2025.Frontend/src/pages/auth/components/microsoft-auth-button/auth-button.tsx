import { faMicrosoft } from '@fortawesome/free-brands-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { FC } from 'react';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';

import { ToastModeEnum } from '@/common';
import { BaseButton } from '@/components';
import { useToast } from '@/hooks';
import { useMsalAuth } from '@/hooks/auth';
import { IMicrosoftAuthRequestDto } from '@/models/requests';
import { useMicrosoftMutation } from '@/services';
import { setProfile } from '@/store/auth';

type AuthButtonProps = {}

const AuthButton: FC<AuthButtonProps> = () => {

    const { login } = useMsalAuth();
    const [microsoftLogIn] = useMicrosoftMutation();
    const navigate = useNavigate();
    const { addToast } = useToast();
    const dispatch = useDispatch();

    const handleLogin = async (): Promise<void> => {
        try {
            const accessToken = await login();
            const requestData: IMicrosoftAuthRequestDto = { accessToken };

            const response = await microsoftLogIn(requestData).unwrap();
            dispatch(setProfile({ id: response.data.id }));
            navigate('/');
        } catch (error: any) {
            addToast(ToastModeEnum.ERROR, `Login failed: ${error.message}`);
        }
    };

    return (
        <BaseButton onClick={handleLogin}>
            Login with Microsoft <FontAwesomeIcon icon={faMicrosoft}/>
        </BaseButton>
    );
};

export { AuthButton };
