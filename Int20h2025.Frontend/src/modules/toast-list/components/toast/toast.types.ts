import { ToastModeEnum } from '@/common/enums';

type ToastType = {
    id: string,
    title: string,
    mode: ToastModeEnum,
    delay?: number,
}

export { type ToastType };
