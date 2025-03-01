import { HttpType } from '@/common';
import { IApiResponseDto } from '@/models/responses';
import { apiSlice } from '@/services';

export const profileApiSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
        getProfile: builder.query<IApiResponseDto<{ id: string }>, void>({
            query: () => ({
                url: '/api/profile/myprofile',
                method: HttpType.GET,
            }),
        }),
    })
});

export const { useGetProfileQuery } = profileApiSlice;
