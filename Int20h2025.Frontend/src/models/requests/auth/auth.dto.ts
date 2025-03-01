interface IAuthRequestDto {
    email: string;
    password: string;
}

interface IGoogleAuthRequestDto {
    accessToken: string;
}

interface IMicrosoftAuthRequestDto {
    accessToken: string;
}

export { type IAuthRequestDto, type IGoogleAuthRequestDto, type IMicrosoftAuthRequestDto };
