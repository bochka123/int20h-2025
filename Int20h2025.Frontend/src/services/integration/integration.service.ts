import { HttpType } from '@/common';
import { IIntegrationRequestDto } from '@/models/requests';
import { IApiResponseDto } from '@/models/responses';
import { apiSlice } from '@/services';

export const integrationApiSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
        integrate: builder.mutation<IApiResponseDto<null>, IIntegrationRequestDto>({
            query: (data) => ({
                url: '/api/integration/integrate',
                method: HttpType.POST,
                body: data,
            }),
        }),
    })
});

export const { useIntegrateMutation } = integrationApiSlice;
