define(['jquery','easing'],function ($) {
    return {
        elem:{
            $container: $("#nav"),
            $handler: $("#nav-handler"),
            $items: $("#nav>ul>li"),
            $line: $("#nav-line")
        },
        itemSize: $("#nav>ul>li").size(),
        pos:{
            //各菜单项掉下来的最终位置
            itemPosTops:[100, 202, 304, 406, 508],
            //tooltip的起始位置到最终位置
            tipPosLefts:{
                even:[-87, -125],
                odd:[19, 48]
            },
            //竖线最终高度
            lineHeight: 490
        },
        slideDown: function(){
            var that = this;
            var runtime = [
                200,
                400,
                600,
                800,
                1000
            ];
            for(var i = 0; i < this.itemSize; i++){
                //this.elem.$items.eq(i).stop(true,true).animate({top: that.pos.itemPosTops[i], opacity:1}, 1000, "easeOutBounce");
                this.elem.$items.eq(i).stop(true,true).animate({top: that.pos.itemPosTops[i], opacity:1}, runtime[i]);
            }
            //this.elem.$line.stop(true,true).animate({height: that.pos.lineHeight, opacity:1}, 1000, "easeOutBounce");
            this.elem.$line.stop(true,true).animate({height: that.pos.lineHeight, opacity:1}, 1000);
            this.elem.$container.removeClass("close").addClass("open");
        },
        slideUp: function(){
            var that = this;
            for(var i = 0; i < this.itemSize; i++){
                this.elem.$items.eq(i).stop(true, true).animate({top: 0, opacity:0}, 600);
            }
            this.elem.$line.stop(true, true).animate({height: 0, opacity:0}, 600);
            this.elem.$container.removeClass("open").addClass("close");
        },
        showTooltip: function($elem, index){
            var that = this;
            var oddeven = (index%2) ? "odd" : "even";
            $elem.stop(true,true).css("display","block").animate({left: that.pos.tipPosLefts[oddeven][1], opacity:1});
        },
        hideTooltip: function($elem, index){
            var that = this;
            var oddeven = (index%2) ? "odd" : "even";
            $elem.stop(true, true).animate({left: that.pos.tipPosLefts[oddeven][0], opacity: 0}, function(){$(this).css("display","none")});
        },
        setCurrent: function(index){
            var $lasttip = this.elem.$items.children(".current").find("span");
            var lastIndex = this.elem.$items.children(".current").parent().index();
            var $a = this.elem.$items.children("a").removeClass("current").eq(index).addClass("current");
            var $tooltip = $a.find("span");
            this.showTooltip($tooltip, index);
            this.hideTooltip($lasttip, lastIndex);
        },
        getCurrentIndex: function(){
            return this.elem.$items.children(".current").parent().index();
        },
        bindEvent:{
            clickHandler: function(){
                var that = this;
                this.elem.$handler.click(function(e){
                    e.preventDefault();
                    var ifclose = that.elem.$container.hasClass("close");
                    if(ifclose){
                        that.slideDown();
                    }else{
                        that.slideUp();
                    }
                });
            },
            tooltipHandler: function(){
                var that = this;
                this.elem.$items.hover(function(){
                        var index = $(this).index();
                        var $elem = $(this).find("span");
                        that.showTooltip($elem, index);
                    }, function(){
                        if(!$(this).children("a").hasClass("current")) {
                            var index = $(this).index();
                            var $elem = $(this).find("span");
                            that.hideTooltip($elem, index);
                        }
                });
            }
        },

        init: function(){
            this.bindEvent.clickHandler.call(this);
            this.bindEvent.tooltipHandler.call(this);
            this.slideDown();
            this.setCurrent(0);
        }
    };
});