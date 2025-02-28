interface IApiResponseDto<T> {
    ok: boolean;
    message: string;
    data: T;
}

export { type IApiResponseDto };
