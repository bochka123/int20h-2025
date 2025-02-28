interface IAuthRequestDto {
    email: string;
    password: string;
}

interface IGoogleAuthRequestDto {
    accessToken: string;
}

export { type IAuthRequestDto, type IGoogleAuthRequestDto };
