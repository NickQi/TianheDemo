define(['jquery','chartsmod/charts', 'chartsmod/appDinge', 'chartsmod/appFenjie', 'chartsmod/appColumn','mod/noticeSlider', 'mod/updateNumber', 'mod/nav', 'mod/bar', 'mod/areasSlider', 'mod/panel', 'easing'],function ($, charts, appDinge, appFenjie, appColumn, NoticeSlider, UpdateNumber, nav, bar, AreasSlider, Panel) {
    function Index(){
        //用能规划定额图表
        this.chartDingeData = null;
        this.chartDinge = null;
        //用电量饼图
        this.chartPieData = null;
        this.chartPie = null;
        //柱状图一
        this.chartcol_1Data = null;
        this.chartcol_1 = null;
        //柱状图二
        this.chartcol_2Data = null;
        this.chartcol_2 = null;
        //节能通告
        this.noticeSlider = null;
        //滚动增加效果的数字
        this.numToUpdate = null;
        //重点区域图
        this.areasInfos = null;
        this.itemsArray = null;
        this.panel1 = new Panel("#panel-1");
        this.panel2 = new Panel("#panel-2");
        this.panel3 = new Panel("#panel-3");
        this.panel4 = new Panel("#panel-4");
        this.panel5 = new Panel("#panel-5");
        this.panels = [this.panel1, this.panel2, this.panel3, this.panel4, this.panel5];
        this.init();
    }

    Index.prototype = {

        init: function(){
            this.render();
        },

        render: function(){
            var that = this;
            this.ajaxHandler();
            this.slideNotice();
            this.updateNum();
            this.initNav();
            //this.activeBar();
            this.initAreasInfos();
            this.panel1.show(0, function(){that.chartDinge = that.renderGauge("chart-1", that.chartDingeData);});
        },

        ajaxHandler: function(param){
            var that = this;
            $.ajax({
                url: this.getAction().dataUrl,
                dataType: 'json',
                type: 'GET',
                data:{
                    Inputs:param || {}
                },
                success: function(json){
                    if(json.ActionInfo && json.ActionInfo.Success){
                        that.chartDinge = that.renderGauge("chart-1", json.Dinge);
                        //初始从0开始
                        var startSeries = {
                            data:[0.00]
                        };
                        that.chartDinge.series[0].update(startSeries);
                        that.chartDingeData = json.Dinge;
                        that.chartPie = that.renderPie("chart-2", json.Pie);
                        that.chartPieData = json.Pie;
                        that.chartcol_1 = that.renderCol("chart-3", json.column1);
                        that.chartcol_1Data = json.column1;
                        that.chartcol_2 = that.renderCol("chart-4", json.column2);
                        that.chartcol_2Data = json.column2;
                    }
                }
            });
        },

        //用能规划定额图表
        renderGauge: function(container, chartData) {
            charts.resetTimezone();
            return appDinge.renderTo(container, chartData);

        },

        //用电量占比饼图
        renderPie: function(container, chartData){
            return appFenjie.renderTo(container, chartData);
        },

        //柱状图:
        renderCol: function(container, chartData){
            return appColumn.renderTo(container, chartData);
        },

        //节能通告滑动
        slideNotice: function(){
            var that = this;
            this.noticeSlider = new NoticeSlider();
            //绑定控制点的点击事件
            $(document).on("click", "#sliderCtrl>li", function(){
                //取消自动滑动
                that.noticeSlider.cancelAutoSlide();
                var index = $(this).index();
                that.noticeSlider.slideTo(index);
                //重新自动滑动
                that.noticeSlider.autoSlide();
            });
        },

        //数字滚动增加
        updateNum: function(){
            this.numToUpdate = new UpdateNumber("#day-num");
        },

        //bar
        activeBar: function(){
            bar.processTo(286);
        },

        //重点区域环境情况
        initAreasInfos: function(){
            this.areasInfos = new AreasSlider();
            this.itemsArray = this.areasInfos.itemsArray;
        },

        //初始化菜单
        initNav: function(){
            var that = this;
            nav.init();
            var callbacks = [
                function(){
                    //that.chartDinge.redraw();
                    that.chartDinge = that.renderGauge("chart-1", that.chartDingeData);
                },
                null,
                function(){
                    that.updateNum();
                },
                function(){
                    //that.chartPie.redraw();
                    that.chartPie = that.renderPie("chart-2", that.chartPieData);
                    //that.chartcol_1.redraw();
                    that.chartcol_1 = that.renderCol("chart-3", that.chartcol_1Data);
                    //that.chartcol_2.redraw();
                    that.chartcol_2 = that.renderCol("chart-4", that.chartcol_2Data);
                },
                function(){
                    that.activeBar();
                    that.areasInfos.resetItemHeights(that.itemsArray);
                    //that.initAreasInfos();
                }
            ];
            var beforeshows = [
                //初始化的时候仪表盘指针指向0
                function(){
                    var startSeries = {
                        data:[0.00]
                    };
                    that.chartDinge.series[0].update(startSeries);
                },
                null,
                function(){
                    that.numToUpdate.resetNum();
                },
                null,
                function(){
                    that.areasInfos.clearItemHeight(that.itemsArray);
                }
            ];
            nav.elem.$items.click(function(e){
                e.preventDefault();
                var index = $(this).index();
                var lastIndex = nav.getCurrentIndex();
                //点击当前已经是current的项目不操作
                if(index !== lastIndex) {
                    that.panels[index].show(1000, callbacks[index], beforeshows[index]);
                    nav.setCurrent(index);
                }
            });
        },


        //Ajax Request
        getAction: function(){
            return {
                dataUrl: '../js/data/data.js'
            }
        }
    };

    return Index;
});