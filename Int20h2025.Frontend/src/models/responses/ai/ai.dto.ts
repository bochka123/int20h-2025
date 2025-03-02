interface IMessageDto {
    message: string;
}

interface IProcessResponseDto {
    clarification: string;
}

interface IHistoryItemDto {
    text: string;
    result: string;
    success: boolean;
}

export { type IHistoryItemDto, type IMessageDto, type IProcessResponseDto };
