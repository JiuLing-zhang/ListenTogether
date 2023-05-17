window.GetElementHeightFromClass = (css) => {
    let elements = document.getElementsByClassName(css);
    if (elements.length == 0) {
        return 0;
    }
    return elements[0].offsetHeight;
};
