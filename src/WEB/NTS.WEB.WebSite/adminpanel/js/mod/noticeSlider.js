define(['jquery'], function ($) {
    function NoticeSlider(container){
        this.container = container || "#notice-container";
        this.$container = $(this.container);
        this.$items = this.$container.children(".item");
        //控制点容器
        this.$ctrlUL = null;
        //计时器
        this.t = null;
        this.init();
    }
    NoticeSlider.prototype = {
        init: function(){
            this.setTotalWidth();
            this.$ctrlUL = this.addCtrlPoints();
            this.autoSlide();
        },

        //设置容器总宽度
        setTotalWidth: function(){
            var totalSize = this.getItemSize();
            var totalNum = this.getItemWidth();
            this.$container.width(totalNum * totalSize);
        },

        //获取条目总数
        getItemSize: function(){
            return this.$items.size();
        },

        //获取单个条目宽度
        getItemWidth: function(){
            return this.$items.outerWidth(true);
        },

        //添加控制点
        addCtrlPoints: function(){
            var $ul = $("<ul id='sliderCtrl'></ul>");
            var htmlLi = "";
            var total = this.getItemSize();
            for(var i = 0; i < total; i++){
                htmlLi += "<li>" + (i+1) + "</li>";
            }
            $ul.html(htmlLi);
            //将第一项设为当前项
            $ul.children("li").eq(0).addClass("active");
            this.$container.after($ul);
            return $ul;
        },

        //获取当前index
        getCurrentIndex: function(){
            return this.$ctrlUL.children(".active").index();
        },

        //滑动
        slideTo: function(index){
            var itemWidth = this.getItemWidth();
            var marginLeft = -itemWidth * index;
            this.$container.animate({marginLeft: marginLeft});
            //将对应的控制点改为当前状态
            this.$ctrlUL && this.$ctrlUL.children("li").removeClass("active").eq(index).addClass("active");
        },

        //自动滑动
        autoSlide: function(){
            var that = this;
            var fn = function(){
                var index = this.getCurrentIndex();
                var total = this.getItemSize();
                index++;
                if(index >= total){
                    index = 0;
                }
                this.slideTo(index);
            };
            this.t = setInterval(function(){fn.call(that)}, 5000);
        },
        //取消自动滑动
        cancelAutoSlide: function(){
            clearInterval(this.t);
        }
    };
    return NoticeSlider;
});