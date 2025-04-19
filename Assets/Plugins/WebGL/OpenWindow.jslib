mergeInto(LibraryManager.library, {
    OpenJSLink: function (url) {
        window.open(Pointer_stringify(url));
    }
});