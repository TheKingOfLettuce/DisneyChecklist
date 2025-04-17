mergeInto(LibraryManager.library, {
  SaveToLocalStorage: function (keyPtr, valuePtr) {
    const key = UTF8ToString(keyPtr);
    const value = UTF8ToString(valuePtr);
    try {
      localStorage.setItem(key, value);
    } 
    catch (e) {
      console.error("Failed to save to localStorage:", e);
    }
  },

  LoadFromLocalStorage: function (keyPtr) {
    const key = UTF8ToString(keyPtr);
    const value = localStorage.getItem(key);
    if (value) {
      const buffer = lengthBytesUTF8(value) + 1;
      const ptr = _malloc(buffer);
      stringToUTF8(value, ptr, buffer);
      return ptr;
    }
    return 0;
  }
});