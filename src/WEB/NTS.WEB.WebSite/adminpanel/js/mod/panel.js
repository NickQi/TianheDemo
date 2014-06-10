define(['jquery', 'easing'], function ($) {
    function Panel(containerId){
        this.$container = $(containerId);
    }
    Panel.prototype = {
        show: function(delay, callback, beforeShow){
            var callbackfunc = function(){
                if(callback && typeof callback === "function"){
                    callback();
                }
            };
            if(beforeShow && typeof beforeShow === "function"){
                beforeShow();
            }
            //之前已经显示在屏幕上的panel上升
            $(".currentPanel").removeClass("currentPanel").stop(true, true).animate({top: 100, opacity:0}, 1000, "easeInBack");
            this.$container.addClass("currentPanel").stop(true, true).delay(delay).animate({top: 220, opacity:1}, 1000, callbackfunc);
        }
    };

    return Panel;
});