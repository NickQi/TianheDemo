define(['jquery', 'mod/areaItem'], function ($, AreaItem) {
    function AreasSlider(opts){
        this.defaultOpts = {
            $container: $("#areasInfoContainer"),
            $items: $("#areasInfoContainer").children(".item"),
            $prevBtn: $("#areasPrev>a"),
            $nextBtn: $("#areasNext>a"),
            showNum: 3
        };
        this.settings = $.extend({}, this.defaultOpts, opts);
        this.offsetNum = 0;
        this.itemsArray = null;
        this.init();
    }
    AreasSlider.prototype = {
        init: function(){
            //首先实例化每一项，放进数组
            this.itemsArray = this.initItems();
            this.setWidth();
            this.setSpecialClass();
            this.bindEvents.nextHandler.call(this);
            this.bindEvents.prevHandler.call(this);
            this.bindEvents.itemHoverHandler.call(this);
        },

        getItemsSize: function(){
            return this.settings.$items.size();
        },

        getItemWidth: function(){
            return this.settings.$items.outerWidth(true);
        },

        setWidth: function(){
            var itemWidth = this.getItemWidth();
            var itemsSize = this.getItemsSize();
            this.settings.$container.width(itemWidth * itemsSize);
        },

        setSpecialClass: function(){
            var specialIndex = this.settings.showNum + this.offsetNum - 1;
            this.settings.$items.removeClass("spec").eq(specialIndex).addClass("spec");
        },

        slide: function(direction){
            var itemSize = this.getItemsSize();
            var itemWidth = this.getItemWidth();
            var showNum = this.settings.showNum;
            var maxOffsetLeftNum = itemSize - showNum;
            //var offsetNum = 0;
            switch (direction){
                case "prev":
                    this.offsetNum = (this.offsetNum - 1) < 0 ? 0 : this.offsetNum - 1;
                    break;
                case "next":
                    this.offsetNum = (this.offsetNum + 1) > maxOffsetLeftNum ? maxOffsetLeftNum : this.offsetNum + 1;
                    break;
            }
            this.settings.$container.stop(true, true).animate({marginLeft: -itemWidth * this.offsetNum});
            //show第一个可见的图的tooltip
            this.showSingleTip(this.offsetNum);
            //按钮样式
            if(this.offsetNum === 0){
                this.settings.$prevBtn.addClass("disabled");
                this.settings.$nextBtn.removeClass("disabled");
            }else if(this.offsetNum === maxOffsetLeftNum){
                this.settings.$prevBtn.removeClass("disabled");
                this.settings.$nextBtn.addClass("disabled");
            }else{
                this.settings.$prevBtn.removeClass("disabled");
                this.settings.$nextBtn.removeClass("disabled");
            }
            this.setSpecialClass();
        },

        bindEvents:{
            prevHandler: function(){
                var that = this;
                this.settings.$prevBtn.click(function(e){
                    e.preventDefault();
                    that.slide("prev");
                });
            },
            nextHandler: function(){
                var that = this;
                this.settings.$nextBtn.click(function(e){
                    e.preventDefault();
                    that.slide("next");
                });
            },
            //个对象鼠标悬停后显示对应的TIP
            itemHoverHandler: function(){
                var that = this;
                $(".colorsDot").mouseover(function(){
                    //var index = $(this).index();
                    var index = $(".colorsDot").index(this);
                    that.showSingleTip(index);
                });
            }
        },

        initItems: function(){
            var $itemContainers = this.settings.$items;
            var itemsArray = [];
            for(var i = 0; i < $itemContainers.size(); i++){
                itemsArray.push(new AreaItem($itemContainers.eq(i)));
            }
            this.resetItemHeights(itemsArray);
            return itemsArray;
        },

        resetItemHeights: function(itemsArray){
            var particleNums = [4,5,3,4,5,3];
            for(var i = 0; i < itemsArray.length; i++){
                var showTip = false;
                if(i === 0){
                    showTip = true;
                }
                itemsArray[i].resetHeight(particleNums[i], showTip);
            }
        },
        clearItemHeight: function(itemsArray){
            for(var i = 0; i < itemsArray.length; i++){
                itemsArray[i].resetHeight(0, false);
            }
        },

        showSingleTip: function(index){
            var $items = this.itemsArray;
            for(var i = 0; i < $items.length; i++){
                if(i === index){
                    $items[i].showTip();
                }else{
                    $items[i].hideTip();
                }
            }
        }
    };

    return AreasSlider;
});