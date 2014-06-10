/*
 * Async Treeview 0.1 - Lazy-loading extension for Treeview
 *
 * http://bassistance.de/jquery-plugins/jquery-plugin-treeview/
 *
 * Copyright 2010 Jörn Zaefferer
 * Released under the MIT license:
 *   http://www.opensource.org/licenses/mit-license.php
 */

;(function($) {

function load(settings, root, child, container) {
	function createNode(parent) {
		var current = $("<li/>").attr("id", this.id || "").html("<span>" + this.text + "</span>").appendTo(parent);
		if (this.classes) {
			current.children("span").addClass(this.classes);
		}
		if (this.expanded) {
			current.addClass("open");
		}
		if (this.hasChildren || this.children && this.children.length) {
			var branch = $("<ul/>").appendTo(current);
			if (this.hasChildren) {
				current.addClass("hasChildren");
				createNode.call({
					classes: "placeholder",
					text: "&nbsp;",
					children:[]
				}, branch);
			}
			if (this.children && this.children.length) {
				$.each(this.children, createNode, [branch])
			}
		} else {
			current.children("span").unbind().bind('click', function() {
		        if (typeof settings.callbacks.select === "function") {
		        	settings.callbacks.select.call(this);
		        }
			});
		}
	}
   //root = JSON.stringify(root);
   var root = JSON.stringify(root);
   // root = [ { "title": "One", "key": "1" }, { "title": "Two", "key": "2" } ];
   //console.log(root);
	$.ajax($.extend(true,settings.ajax, {
		url: settings.url,
		dataType: "json",
        type: "POST",
		data: {Inputs:root},
		success: function(response) {
			child.empty();
			$.each(response, createNode, [child]);
	        $(container).treeview({add: child});
	        if (typeof settings.callbacks.scroll === "function") {
	        	settings.callbacks.scroll.call(this);
	        }
	    }
	}));
}

var proxied = $.fn.treeview;
$.fn.treeview = function(settings) {
if (!settings.url) {
return proxied.apply(this, arguments);
}

var container = this;
//if (!container.children().size())
load(settings, settings.root, this, container);
var userToggle = settings.toggle;
return proxied.call(this, $.extend({}, settings, {
collapsed: true,
toggle: function() {
var $this = $(this);
if ($this.hasClass("hasChildren")) {
var childList = $this.removeClass("hasChildren").find("ul");
$this.removeClass("hasChildren");
//childList.empty();
settings.url="action.ashx?action=devicetree";
//post JSON
//alert($(".icons-industry a.current").attr("data-id"));
var data = {
"AreaID":parseInt(this.id),
"ItemCode":$(".icons-industry a.current").attr("data-id")
};
load(settings, data, childList, container);
}
if (userToggle) {
userToggle.apply(this, arguments);
}
}
}));
};
})(jQuery);


