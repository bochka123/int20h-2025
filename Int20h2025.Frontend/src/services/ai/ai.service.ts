import { HttpType } from '@/common';
import { IProcessRequestDto } from '@/models/requests';
import { IApiResponseDto, IHistoryItemDto, IProcessResponseDto } from '@/models/responses';
import { apiSlice } from '@/services';

export const aiApiSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
        process: builder.mutation<IApiResponseDto<IProcessResponseDto>, IProcessRequestDto>({
            query: (data) => ({
                url: '/api/ai/process',
                method: HttpType.POST,
                body: data,
            }),
        }),
        getHistory: builder.query<IApiResponseDto<IHistoryItemDto[]>, void>({
            query: () => ({
                url: '/api/prompt',
                method: HttpType.GET,
            }),
        }),
    })
});

export const { useProcessMutation } = aiApiSlice;
export const { useGetHistoryQuery } = aiApiSlice;
