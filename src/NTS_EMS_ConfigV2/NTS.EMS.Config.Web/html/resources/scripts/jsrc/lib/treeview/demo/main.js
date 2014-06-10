/**
 * ==========================================================
 * Copyright (c) 2013, NTS-9000 All rights reserved.
 * NTS项目建筑统计JS
 * Author: Jinsam
 * Date: 2013-06-04 21:26:27 615000
 * ==========================================================
 */

require.config({
	baseUrl: "/jsrc",
	urlArgs: "",
	paths: {
		jquery: "lib/jquery/jquery.min",
		artTemplate: "lib/artTemplate/template.min",
		blockui: "lib/blockUI/blockUI",
		datepicker: "lib/datepicker/datepicker",
		treeview: "lib/treeview/treeview",
		treeviewEdit: "lib/treeview/treeview_edit",
		treeviewAsync: "lib/treeview/treeview_async",
		common: "common/common",
		menuExtend: "view/menu_extend"
	},
	shim: {
		datepicker: ["jquery"],
		treeview: ["jquery"],
		treeviewEdit: ["treeview"],
		treeviewAsync: ["treeview", "treeviewEdit"]
	}
});

require(["jquery", "artTemplate", "blockui", "datepicker", "treeview", "treeviewAsync", "treeviewEdit", "common", "menuExtend"], 
function($, ArtTemplate, BlockUI, datepicker, TreeView, TreeViewAsync, TreeViewEdit, Common, MenuExtend) {
	this.common = new Common();
	this.menuExtend = new MenuExtend();
});