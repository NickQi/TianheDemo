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
		//tree: 'lib/tree/tree',
		drilldown: 'lib/highcharts/modules/drilldown',
		chartsmod: 'chartsmod',
		common: 'common',
		contrast: 'view/energy_contrast'
	},
	shim: {
		common: ['base'],
		blockui: ['base'],
		jscrollpane: ['base'],
		mousewheel: ['base'],
		datepicker: ['base'],
		highcharts: ['base'],
		//tree: ['base'],
		drilldown: ['highcharts'],
		order: ['base', 'blockui', 'jscrollpane', 'mousewheel']
	}
});

require(['base', 'blockui', 'jscrollpane', 'mousewheel', 'datepicker', 'common', 'contrast'], function($, blockui, jscrollpane, mousewheel, datepicker, Common, Contrast) {
	this.contrast = new Contrast();
});