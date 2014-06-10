define(['jquery'], function ($) {
    function UpdateNum(container){
        this.$container = $(container);
        this.init();
    }
    UpdateNum.prototype = {
        init: function(){
            //var num = this.getValue();
            //this.sliceNumber(num);
            //this.updateHandler($("#day-num>span").eq(1), 7);
            this.rolling();
        },

        //获取数值
        getValue: function(){
            //return parseInt(this.$container.html());
            return parseInt(this.$container.attr("data-endnum"));
        },

        //拆分数字填入各自的元素
        sliceNumber: function(num){
            //var num = this.getValue();
            var numArray = num.toString().split("");
            var htmlNum = "";
            for(var i = 0; i < numArray.length; i++){
                htmlNum += "<span>" + numArray[i] + "</span>";
            }
            this.$container.html(htmlNum);
            return numArray;
        },

        //翻滚吧，数字！
        rolling: function(){
            var num = this.getValue();
            //var numArray = this.sliceNumber(num);
            var numArray = [num];
            for(var i = 0; i < numArray.length; i++){
                //this.updateHandler(this.$container.children("span").eq(i), numArray[i]);
                this.updateHandler(this.$container, numArray[i]);
            }
        },

        //数字递增
        updateHandler: function(container, endNumber){
            var that = this;
            var $container = container;
            var endNum = parseInt(endNumber);
            var startNum = parseInt($container.html());
            if(startNum == endNumber){
                startNum = -1;
            }
            var num = ++startNum;
            $container.html(num);
            var atime = 2000;
            var intervaltime = atime/(endNum + 1);
            if(num < endNum){
                setTimeout(function(){that.updateHandler.call(that, $container, endNum)}, intervaltime);
            }
        },
        resetNum: function(){
            this.$container.html(0);
        }

    };

    return UpdateNum;
});