/**
 * ==========================================================
 * Copyright (c) 2013, tiansu-china.com All rights reserved.
 * 充值缴费详情页APP
 * Author: Jinsam
 * Date: 2014-01-01 22:59:08 549899
 * ==========================================================
 */


require.config({
	urlArgs: '',
	baseUrl: '/js/',
	paths: {
		base: 'base',
		blockui: 'lib/blockui/blockui.min',
		mousewheel: "lib/jscrollpane/mousewheel",
		highcharts: 'lib/highcharts/highcharts.src',
		//tree: 'lib/tree/tree',
		drilldown: 'lib/highcharts/modules/drilldown',
		chartsmod: 'chartsmod',
		common: 'common',
		index: 'view/index'
	},
	shim: {
		common: ['base'],
		blockui: ['base'],
		mousewheel: ['base'],
		highcharts: ['base'],
		//tree: ['base'],
		drilldown: ['highcharts'],
		index: ['base', 'blockui', 'mousewheel']
	}
});

require(['base', 'blockui', 'mousewheel', 'common', 'index'], function($, blockui, mousewheel, Common, Index) {
	this.index = new Index();
});