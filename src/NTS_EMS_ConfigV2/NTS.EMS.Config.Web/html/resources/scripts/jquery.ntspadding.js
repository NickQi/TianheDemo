(function ($) {
    var $thisObj;
    $.fn.ntspadding = function (options) {
        var opts = $.extend({}, $.fn.ntspadding.defaults, options);
        return this.each(function () {
            $this = $(this);
            var o = $.meta ? $.extend({}, opts, $this.data()) : opts;
            var selectedpage = o.start;
            $.fn.draw(o, $this, selectedpage);
            $thisObj = o;
        });
    }; 
    $.fn.ntspadding.defaults = {
        recordnum: 0,
        count: 5, // 总页数
        pagesize: 10,
        currentclass: 'current',
        onChange: function () { return false; },
        resetClass: function (o, j) {
            $(o).find('li').each(function () {
                //alert($(this).find('a').html());
                //alert($(this).find('a').length);

                if ($(this).find('a').length == 0) {
                    if (o.currentclass == undefined) {

                        $(this).removeClass("current").html('<a >' + $(this).html() + '</a>');
                    } else {
                        $(this).removeClass(o.currentclass).html('<a >' + $(this).html() + '</a>');
                    }
                }
                $(this).removeClass(o.currentclass);
            });
        }
    };
    $.ReSetting = function (options) {
        $thisObj.recordnum = options.recordnum;
        $thisObj.count = options.count;
        $.fn.draw($thisObj);
        var currval = options.page;
        var $thisPage = $("#pages");
        var $thisselect = $('#pselect');
         $thisPage.val(currval);
         $thisselect.val(currval);
        
    };
    $.fn.draw = function (o, obj, selectedpage) {
        if (o.display > o.count)
            o.display = o.count;
        $this.empty();

        var _first = $(document.createElement('a')).html('&laquo; 首页').css("cursor", "pointer");

        var divwrapleft = $(document.createElement('li'));
        divwrapleft.append(_first);
        //		
        var ulwrapdiv = $(document.createElement('div')).addClass("pagination pagination-left");


        // 编写记录数的基本信息
        var pageinfo = '共' + o.recordnum + '条记录';
        var recorddiv = $(document.createElement('div')).addClass('results').html("<span>" + pageinfo + "</span><input type='hidden' id='pages' value='1'>");
        var ul = $(document.createElement('ul')).addClass('pager').css("overflow", "hidden");
        ulwrapdiv.append(recorddiv); // 记录数基本情况

        // _ulwrapdiv.append(_divwrapleft); // 首页
        ul.append(divwrapleft); // 首页


        //var c = (o.display - 1) / 2;
       // var first = selectedpage - c;
        var selobj = $(document.createElement('select')).attr('id','pselect');
       // selobj.options.clear();
        for (var i = 1; i < o.count+1; i++) {
            selobj.append("<option value=" + i + ">第" + i + "页</option>");
        }
//        for (var i = 0; i < o.count; i++) {
//            var val = i + 1;
//            var _obj;
//            if (val == selectedpage) {
//                _obj = $(document.createElement('li')).addClass(o.currentclass).html(val);
//                ul.append(_obj);
//            } else {
//                _obj = $(document.createElement('li')).html('<a>' + val + '</a>');
//                ul.append(_obj);
//            }
        //        }
        var prev;
        if ($("#pages").val() == 1) {
            prev = $(document.createElement('a')).html('上一页').attr('disabled', true).css("cursor","pointer");
        } else {
            prev = $(document.createElement('a')).html('上一页').css("cursor", "pointer");
        }
       // var prev = $(document.createElement('a')).html('上一页');
        var prevdivwrapright = $(document.createElement('li'));
        prevdivwrapright.append(prev);
        ul.append(prevdivwrapright);

        var next;
        if ($("#pages").val() == o.count) {
            next = $(document.createElement('a')).html('下一页').attr('disabled', true).css("cursor", "pointer");
        } else {
            next = $(document.createElement('a')).html('下一页').css("cursor", "pointer");
        }
        var nextdivwrapright = $(document.createElement('li'));
        nextdivwrapright.append(next);
        ul.append(nextdivwrapright);

        var last = $(document.createElement('a')).html('尾页 &raquo;').css("cursor", "pointer");
        var divwrapright = $(document.createElement('li'));
        divwrapright.append(last);
        ul.append(divwrapright);



        var selectdiv = $(document.createElement('div')).attr('config', 's').css({'display':'inline-block','padding-top':'4px'});
;
        selectdiv.append("<span>&nbsp;&nbsp;转到：&nbsp;&nbsp;</span>");
        selectdiv.append(selobj);
        selectdiv.append("<span>&nbsp;&nbsp;</span>");
        ul.append(selectdiv);

        ulwrapdiv.append(ul);
        //append all:
        $this.append(ulwrapdiv);

        ulwrapdiv.find('select').change(function () {
            var currval = $(this).val();
            var $pages = $("#pages");
            $pages.val(currval);
            var data = { page: currval, pagesize: o.pagesize };
            o.onChange(data);
        });
        //click a page
        ulwrapdiv.find('li[config!="s"]').click(function (e) {
            var $pages = $("#pages");
            o.resetClass(ulwrapdiv, $(this));
            var currval = $(this).find('a').html();
            $(this).addClass(o.currentclass).html(currval).css("cursor", "pointer");
            if (currval == '« 首页') { currval = 1; }
            if (currval == '上一页') {
                if ($pages.val() == 1) {
                    alert('对不起，已经到第一页了');
                    return;
                }else{ currval = Number($pages.val()) - 1;}
            }
            if (currval == '下一页') {
                if ($pages.val() == o.count) {
                    alert('对不起，已经到尾页了'); return;
                } else {
                    currval = Number($pages.val()) + 1;
                }
            }
            if (currval == '尾页 »') { currval = o.count; }
            $pages.val(currval);
            $('#pselect').val(currval);
            var data = { page: currval, pagesize: o.pagesize };
            o.onChange(data);
            
        });


    };
})(jQuery);