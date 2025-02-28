const getMessage = (error: any): string | null => {
    if (typeof error !=='object') return null;
    for (const [_, value] of Object.entries(error)) {
        if (value && typeof value === 'object' && ('message' in value) && value.message) {
            return value.message as string;
        } else if (Array.isArray(value)) {
            for (const item of value) {
                const message = getFormErrorMessage(item);
                if (message) return message;
            }
        }
    }
    return null;
};

const getFormErrorMessage = (error: object): string => {
    const message = getMessage(error);
    return  message || 'Failed form validation';
};

export { getFormErrorMessage };
