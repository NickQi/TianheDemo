
function loadPage() {
   //this.init();
    }

    loadPage.prototype = {
        pagesize: 10,
        /**
        * 初始化页面
        * author: wyl
        */
        init: function () {
            this.render();
        },

        /**
        * 渲染页面
        */
        render: function () {
            this.bindEvent.loadPagePanel.call(this);
        },

        /**
        * 页面事件绑定
        */
        bindEvent: {

            /** 
            * 保存、查询事件
            */
            loadPagePanel: function () {

                var self = this;

            }
        },

        getinfo: function () {
            this.searchAction(1, false);
        },

        searchAction: function (currentPage, isPadding) {

            var data = {
                PageCurrent: currentPage,
                PageSize: this.pagesize
            };

            data = JSON.stringify(data);
            jQuery.ajax({
                url: this.getAction().GetUserGroupList,
                dataType: "json",
                type: "POST",
                data: { "Inputs": data },
                success: function (json) {

                    if (json.ResultInfo.Success) {
                        new loadPage().showUserGroupList(json);
                    } else {
                        alert(json.ResultInfo.ExceptionMsg);
                    }
                    if (!isPadding) {
                        if (json.Page != null) {
                            new loadPage().setPadding(json.Page.Total);
                        }
                        else {
                            new loadPage().setPadding(0);
                        }
                    }
                },
                error: function (msg) {
                    alert("服务器错误，请联系管理员！--411");
                }
            });

        },

        showUserGroupList: function (json) {
            jQuery(".table tbody").empty();
            var $list = jQuery(".table tbody"),
		    html = "";
            if (json.UserGroupList != null && json.UserGroupList.length > 0) {
                html = template.render("userGroupConfigItem", json);
                $list.html(html);
            } else {
                // self.common.loading($list, false);
            }
        },

        setPadding: function (total) {
            var countNum = Math.ceil(total / this.pagesize);
            jQuery("#padding").ntspadding({
                recordnum: total,
                count: countNum,
                onChange: function (data) {
                    new loadPage().searchAction(data.page, true);
                }
            });
        },

        /*
        *dialog dialog-edit
        */


        /**
        * Ajax请求
        */
        getAction: function () {
            return {
                GetUserGroupList: "action.ashx?action=GetUserGroupList",
                SaveUserGroup: "action.ashx?action=SaveUserGroup",
                GetUserGroupInfo: "action.ashx?action=GetUserGroupInfo",
                DeleteUserGroup: "action.ashx?action=DeleteUserGroup",
                IsContainUser: "action.ashx?action=IsContainUser"
            };
        }
    };


  new loadPage().getinfo();