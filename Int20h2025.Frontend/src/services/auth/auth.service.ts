import { HttpType } from '@/common';
import { IAuthRequestDto, IGoogleAuthRequestDto, IMicrosoftAuthRequestDto } from '@/models/requests';
import { IApiResponseDto } from '@/models/responses';
import { apiSlice } from '@/services';


export const authApiSlice = apiSlice.injectEndpoints({
    endpoints: builder => ({
        login: builder.mutation<IApiResponseDto<{ id: string }>, IAuthRequestDto>({
            query: (data) => ({
                url: '/api/auth/login',
                method: HttpType.POST,
                body: data,
            }),
        }),
        register: builder.mutation<IApiResponseDto<{ id: string }>, IAuthRequestDto>({
            query: (data) => ({
                url: '/api/auth/register',
                method: HttpType.POST,
                body: data,
            }),
        }),
        logOut: builder.mutation<IApiResponseDto<null>, void>({
            query: () => ({
                url: '/api/auth/logout',
                method: HttpType.POST,
            }),
        }),
        google: builder.mutation<IApiResponseDto<{ id: string }>, IGoogleAuthRequestDto>({
            query: (data) => ({
                url: '/api/auth/google',
                method: HttpType.POST,
                body: data
            }),
        }),
        microsoft: builder.mutation<IApiResponseDto<{ id: string }>, IMicrosoftAuthRequestDto>({
            query: (data) => ({
                url: '/api/auth/microsoft',
                method: HttpType.POST,
                body: data
            }),
        }),
    })
});

export const { useLoginMutation, useRegisterMutation, useLogOutMutation, useGoogleMutation, useMicrosoftMutation } = authApiSlice;
