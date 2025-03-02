import { Dispatch, FC, SetStateAction } from 'react';

import { Modal } from '@/components';

type SyncTrelloModalProps = {
    visible: boolean;
    setVisible: Dispatch<SetStateAction<boolean>>;
}
const SyncTrelloModal: FC<SyncTrelloModalProps> = ({ visible, setVisible }) => {
    return (
        <Modal heading={'Synchronize with Trello'} visible={visible} setVisible={setVisible}>
            test
        </Modal>
    );
};

export { SyncTrelloModal };
