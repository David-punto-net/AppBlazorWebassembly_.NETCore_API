export function initialize(dotNetHelper, element) {
    const resizeObserver = new ResizeObserver(() => {
        dotNetHelper.invokeMethodAsync('UpdateItemsPerPage');
    });

    resizeObserver.observe(element);

    element.addEventListener('transitionend', () => {
        dotNetHelper.invokeMethodAsync('HandleTransitionEnd');
    });

    return {
        dispose: () => {
            resizeObserver.disconnect();
            element.removeEventListener('transitionend');
        }
    };
}

export function resetPosition(element, translateX) {
    element.style.transition = 'none';
    element.style.transform = `translateX(-${translateX}%)`;
    void element.offsetHeight; // Trigger reflow
    element.style.transition = '';
}

export function getWindowWidth() {
    return window.innerWidth;
} crollCarousel;