import { HttpType } from '@/common';
import { IProcessRequestDto } from '@/models/requests';
import { IApiResponseDto } from '@/models/responses';
import { apiSlice } from '@/services';

export const aiApiSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
        process: builder.mutation<IApiResponseDto<null>, IProcessRequestDto>({
            query: (data) => ({
                url: '/api/ai/process',
                method: HttpType.POST,
                body: data,
            }),
        }),
    })
});

export const { useProcessMutation } = aiApiSlice;
