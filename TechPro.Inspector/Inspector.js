(function () {

    var $ = function (sel) {
        return document.querySelectorAll(sel);
    };

    function init() {
        console.log("Inspector booting up");
    }
    
    window.addEventListener('load', init, false);
})();