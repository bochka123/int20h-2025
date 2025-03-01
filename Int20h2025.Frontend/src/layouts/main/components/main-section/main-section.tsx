import { faPaperPlane } from '@fortawesome/free-solid-svg-icons';
import { FC, useState } from 'react';

import { IconButton, MultilineInput } from '@/components';

type MainSectionProps = {}
const MainSection: FC<MainSectionProps> = () => {

    const [message, setMessage] = useState<string>('');

    return (
        <div>
            <MultilineInput value={message} onChange={setMessage} />
            <IconButton icon={faPaperPlane}/>
        </div>
    );
};

export { MainSection };
