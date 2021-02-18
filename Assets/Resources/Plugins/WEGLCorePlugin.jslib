mergeInto(LibraryManager.library, {

    GenerateWebStorage: function(str){
        var pointerString = Pointer_stringify(str);
        if(!localStorage.getItem(pointerString)){
            localStorage.setItem(pointerString,"NOSAVEDATA");
        }
    },
    
    SaveCookies: function (str,key) {
        var pointerToString = Pointer_stringify(str);
        var keyString = Pointer_stringify(key);
        localStorage.setItem(keyString,pointerToString);
    },
    GetCookies: function(key){
        var keyString = Pointer_stringify(key);
        var returnStr = localStorage.getItem(keyString);
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    }
});