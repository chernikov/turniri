
$.debug = function() {
}

$.debug.info = function(message, onServer) {
    $.debug.write(1, message, onServer);
    return (this);
};

$.debug.warn = function (message, onServer) {
    $.debug.write(2, message, onServer);
    return (this);
}

$.debug.error = function (message, onServer) {
    $.debug.write(4, message, onServer);
    return (this);
}

$.debug.write = function(level, message, onServer)
{
    switch (level) {
        case 1:
            var typeString = 'info';
            var addTo = 'messages';
            break;

        case 2:
            var typeString = 'warn';
            var addTo = 'messages';
            break;

        default: case 4:
            var typeString = 'error';
            var addTo = 'errors';
            break;
    }

    // Format time
    var DateObj = new Date();
    var time = DateObj.getHours() + ':' +
                    DateObj.getMinutes() + ':' +
                    DateObj.getSeconds();


    var logMessage = time + ': ' + message;

    // Check console is present
    if (window.console) {
        window.console[typeString](logMessage);
    }       // Check for Opera Dragonfly
    else if ($.browser.opera && window.opera.postError) {
        window.opera.postError(logMessage);
    }

    if (onServer) {
     /*   $.ajax({
            type: "POST",
            url: "/error/index",
            data: "Level=" + level + "&Message=" + message,
        });*/
    }
}