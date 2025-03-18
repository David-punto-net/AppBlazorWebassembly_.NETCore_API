export function GetScrollLeft(id) {
    return document.getElementById(id).scrollLeft
}
export function SetScrollLeft(id, value) {
    document.getElementById(id).scrollLeft = value;
}
export function ElementClientWidth(id, value) {
    return document.getElementById(id).clientWidth
}
export function ElementScrollWidth(id) {
    return document.getElementById(id).scrollWidth
}