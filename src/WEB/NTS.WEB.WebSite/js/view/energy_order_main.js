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
		jscrollpane: "lib/jscrollpane/jscrollpane.min",
		mousewheel: "lib/jscrollpane/mousewheel",
		datepicker: 'lib/datepicker/datepicker',
		highcharts: 'lib/highcharts/highcharts.src',
		drilldown: 'lib/highcharts/modules/drilldown',
		chartsmod: 'chartsmod',
		common: 'common',
		order: 'view/energy_order'
	},
	shim: {
		common: ['base'],
		blockui: ['base'],
		jscrollpane: ['base'],
		mousewheel: ['base'],
		datepicker: ['base'],
		highcharts: ['base'],
		drilldown: ['highcharts'],
		order: ['base', 'blockui', 'jscrollpane', 'mousewheel']
	}
});

require(['base', 'blockui', 'jscrollpane', 'mousewheel', 'datepicker','common', 'order'], function($, blockui, jscrollpane, mousewheel, datepicker, Common, Order) {
	this.order = new Order();
});