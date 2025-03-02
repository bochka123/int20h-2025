import { IntegrationSystemEnum } from '@/common';

interface IIntegrationRequestDto {
    systemName: IntegrationSystemEnum;
    apiKey: string;
    token: string;
}

export { type IIntegrationRequestDto };
