define(['jquery'], function ($) {
    function AreaItem($container){
        this.$container = $container;
        this.$tip = this.$container.find(".colorsInfo");
        this.$colorContent = this.$container.find(".colorsContent");
        this.$hoverDot = this.$container.find(".colorsDot");
        this.particleHeight = 20;
        this.init();
    }
    AreaItem.prototype = {
        init: function(){
            //this.resetHeight();
        },
        resetHeight: function(particleNum, showTip){
            var that = this;
            this.hideTip();
            this.$colorContent.height(0)
                              .stop(true, true)
                              .animate({height: this.particleHeight * particleNum}, 1000, function(){
                                    if(showTip){
                                        that.showTip();
                                    }
                                });
        },
        showTip: function(){
            this.$tip.css("display","block").stop(false, true).animate({opacity: 1});
        },
        hideTip: function(){
            /*this.$tip.stop(false, true).animate({opacity: 0}, function(){
                $(this).css("display","none");
            });*/
            this.$tip.css({display:"none", opacity:"0"});
        }
    };

    return AreaItem;
});