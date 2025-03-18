export function init(dotNetRef, element) {
    const observer = new ResizeObserver(entries => {
        const width = entries[0].contentRect.width;
        let itemsPerPage = 1;

        if (width >= 1300) itemsPerPage = 6; 
        else if (width >= 1000) itemsPerPage = 5; 
        else if (width >= 975) itemsPerPage = 4; 
        else if (width >= 775) itemsPerPage = 3; 
        else if (width >= 590) itemsPerPage = 2; 

        dotNetRef.invokeMethodAsync('UpdateItemsPerPage', itemsPerPage);
    });

    observer.observe(element);

    return {
        dispose: () => {
            observer.unobserve(element);
        }
    };
}
