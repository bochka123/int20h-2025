import { useMsal } from '@azure/msal-react';
import { faMicrosoft } from '@fortawesome/free-brands-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { FC } from 'react';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';

import { ToastModeEnum } from '@/common';
import { loginRequest } from '@/common/configs';
import { BaseButton } from '@/components';
import { useToast } from '@/hooks';
import { IMicrosoftAuthRequestDto } from '@/models/requests';
import { useMicrosoftMutation } from '@/services';
import { setProfile } from '@/store/auth';

type AuthButtonProps = {}

const AuthButton: FC<AuthButtonProps> = () => {

  const { instance } = useMsal();
  const [microsoftLogIn] = useMicrosoftMutation();
  const navigate = useNavigate();
  const { addToast } = useToast();
  const dispatch = useDispatch();

  const login = (): void => {
    instance.loginPopup(loginRequest).then(response => {

      const requestData: IMicrosoftAuthRequestDto = {
        accessToken: response.accessToken
      };

      microsoftLogIn(requestData)
        .unwrap()
        .then((res) => {
          dispatch(setProfile({ id: res.data.id }));
          navigate('/');
        })
        .catch((error) => { addToast(ToastModeEnum.ERROR, `Failed to log in: ${error.message}`); });
    }).catch(error => {
      addToast(ToastModeEnum.ERROR, `Login failed: ${error.message}`);
    });;
  };

  return (
    <BaseButton onClick={login}>Login with Microsoft <FontAwesomeIcon icon={faMicrosoft} /></BaseButton>
  );
};

export { AuthButton };
