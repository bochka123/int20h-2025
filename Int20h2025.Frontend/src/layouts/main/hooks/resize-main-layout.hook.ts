import { RefObject, useEffect } from 'react';

const useResizeMainLayout = (
    mainContentRef: RefObject<HTMLDivElement>,
    leftResizeLineRef: RefObject<HTMLDivElement>,
    rightResizeLineRef: RefObject<HTMLDivElement>,
): void => {
    useEffect(() => {
        if (!leftResizeLineRef || !rightResizeLineRef || !mainContentRef) return;

        let isResizing = false;
        let startX: number;
        let startWidthLeft: number = 300;
        let startWidthRight: number = 300;
        let currentLine: 'left' | 'right' | null = null;

        mainContentRef.current!!.style.gridTemplateColumns = `${startWidthLeft}px 22px auto 22px ${startWidthRight}px`;

        const startResizing = (e: MouseEvent, line: 'left' | 'right'): void => {
            if (!mainContentRef.current) return;

            isResizing = true;
            currentLine = line;
            startX = e.clientX;

            const columns = window.getComputedStyle(mainContentRef.current).gridTemplateColumns.split(' ');
            startWidthLeft = parseInt(columns[0], 10);
            startWidthRight = parseInt(columns[4], 10);

            document.addEventListener('mousemove', resize);
            document.addEventListener('mouseup', stopResizing);
        };

        const resize = (e: MouseEvent): void => {
            if (isResizing && currentLine && mainContentRef.current) {
                let newWidth;

                if (currentLine === 'left') {
                    newWidth = startWidthLeft + (e.clientX - startX);
                    if (newWidth < 200) newWidth = 200;
                    if (newWidth > 600) newWidth = 600;
                    mainContentRef.current.style.gridTemplateColumns = `${newWidth}px 22px auto 22px ${startWidthRight}px`;
                } else if (currentLine === 'right') {
                    newWidth = startWidthRight - (e.clientX - startX); // Зверніть увагу на знак "-"
                    if (newWidth < 200) newWidth = 200;
                    if (newWidth > 600) newWidth = 600;
                    mainContentRef.current.style.gridTemplateColumns = `${startWidthLeft}px 22px auto 22px ${newWidth}px`;
                }
            }
        };

        const stopResizing = (): void => {
            isResizing = false;
            currentLine = null;
            document.removeEventListener('mousemove', resize);
            document.removeEventListener('mouseup', stopResizing);
        };

        leftResizeLineRef.current?.addEventListener('mousedown', (e) => startResizing(e, 'left'));
        rightResizeLineRef.current?.addEventListener('mousedown', (e) => startResizing(e, 'right'));

        return () => {
            leftResizeLineRef.current?.removeEventListener('mousedown', (e) => startResizing(e, 'left'));
            rightResizeLineRef.current?.removeEventListener('mousedown', (e) => startResizing(e, 'right'));
            document.removeEventListener('mousemove', resize);
            document.removeEventListener('mouseup', stopResizing);
        };
    }, []);
};

export { useResizeMainLayout };
