/** 
 * ========================================================== 
 * Copyright (c) 2014, nts All rights reserved.
 * management diagnosis
 * Author: luyy
 * Date: 2014-04-15 21:17:03 317000 
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
		common: 'common',
		diagnosis: 'view/management_diagnosis'
	},
	shim: {
		common: ['base'],
		blockui: ['base'],
		jscrollpane: ['base'],
		datepicker: ['base'],
		tree: ['base'],
		diagnosis: ['base', 'common']
	}
});

require(['base', 'blockui', 'jscrollpane', 'mousewheel', 'datepicker', 'common', 'diagnosis'], function($, blockui, jscrollpane, mousewheel, datepicker, Common, Diagnosis) {
	this.diagnosis = new Diagnosis();
});