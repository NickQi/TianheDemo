define(['jquery','easing'], function ($) {
    return {
        $bar: $("#mainInfoBar"),
        processTo: function(length){
            this.$bar.width(0).stop(true,true).animate({width: length}, 3000, "easeOutCubic").css("overflow","visible")
        }
    };
});