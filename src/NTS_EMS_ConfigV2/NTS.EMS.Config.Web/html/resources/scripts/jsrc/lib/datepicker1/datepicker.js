/* =========================================================
 * bootstrap-datepicker.js
 * http://www.eyecon.ro/bootstrap-datepicker
 * =========================================================
 * Copyright 2012 Stefan Petre
 * Improvements by Andrew Rowls
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * ========================================================= */

! function($) {

	function UTCDate() {
		return new Date(Date.UTC.apply(Date, arguments));
	}

	function UTCToday() {
		var today = new Date();
		return UTCDate(today.getUTCFullYear(), today.getUTCMonth(), today.getUTCDate());
	}

	// Picker object

	var Datepicker = function(element, options) {
		var that = this;

		this.element = $(element);
		this.language = options.language || this.element.data('date-language') || "cn";
		this.language = this.language in dates ? this.language : this.language.split('-')[0]; //Check if "de-DE" style date is available, if not language should fallback to 2 letter code eg "de"
		this.language = this.language in dates ? this.language : "cn";
		this.isRTL = dates[this.language].rtl || false;
		this.format = DPGlobal.parseFormat(options.format || this.element.data('date-format') || dates[this.language].format || 'mm/dd/yyyy');
		this.isInline = false;
		this.isInput = this.element.is('input');
		this.component = this.element.is('.date') ? this.element.find('.add-on, .btn') : false;
		this.hasInput = this.component && this.element.find('input').length;
		if (this.component && this.component.length === 0) this.component = false;

		this.forceParse = true;
		if ('forceParse' in options) {
			this.forceParse = options.forceParse;
		} else if ('dateForceParse' in this.element.data()) {
			this.forceParse = this.element.data('date-force-parse');
		}

		this.picker = $(DPGlobal.template);
		this._buildEvents();
		this._attachEvents();

		if (this.isInline) {
			this.picker.addClass('datepicker-inline').appendTo(this.element);
		} else {
			this.picker.addClass('datepicker-dropdown dropdown-menu');
		}
		if (this.isRTL) {
			this.picker.addClass('datepicker-rtl');
			this.picker.find('.prev i, .next i')
				.toggleClass('icon-arrow-left icon-arrow-right');
		}

		this.autoclose = false;
		if ('autoclose' in options) {
			this.autoclose = options.autoclose;
		} else if ('dateAutoclose' in this.element.data()) {
			this.autoclose = this.element.data('date-autoclose');
		}

		this.keyboardNavigation = true;
		if ('keyboardNavigation' in options) {
			this.keyboardNavigation = options.keyboardNavigation;
		} else if ('dateKeyboardNavigation' in this.element.data()) {
			this.keyboardNavigation = this.element.data('date-keyboard-navigation');
		}

		this.viewMode = this.startViewMode = 0;
		switch (options.startView || this.element.data('date-start-view')) {
			case 2:
			case 'decade':
				this.viewMode = this.startViewMode = 2;
				break;
			case 1:
			case 'year':
				this.viewMode = this.startViewMode = 1;
				break;
		}

		this.minViewMode = options.minViewMode || this.element.data('date-min-view-mode') || 0;
		if (typeof this.minViewMode === 'string') {
			switch (this.minViewMode) {
				case 'months':
					this.minViewMode = 1;
					break;
				case 'years':
					this.minViewMode = 2;
					break;
				default:
					this.minViewMode = 0;
					break;
			}
		}

		this.viewMode = this.startViewMode = Math.max(this.startViewMode, this.minViewMode);

		this.todayBtn = (options.todayBtn || this.element.data('date-today-btn') || false);
		this.todayHighlight = (options.todayHighlight || this.element.data('date-today-highlight') || false);

		// add by ghj 2013-04-06 21:37:11
		this.rangeDate = (options.rangeDate || false);
		this.stepDate = (options.stepDate || false);

		this.calendarWeeks = false;
		if ('calendarWeeks' in options) {
			this.calendarWeeks = options.calendarWeeks;
		} else if ('dateCalendarWeeks' in this.element.data()) {
			this.calendarWeeks = this.element.data('date-calendar-weeks');
		}
		/*if (this.calendarWeeks) this.picker.find('tfoot th.today')
			.attr('colspan', function(i, val) {
			return parseInt(val) + 1;
		});*/

		this._allow_update = false;

		this.weekStart = ((options.weekStart || this.element.data('date-weekstart') || dates[this.language].weekStart || 0) % 7);
		this.weekEnd = ((this.weekStart + 6) % 7);
		this.startDate = -Infinity;
		this.endDate = Infinity;
		this.daysOfWeekDisabled = [];
		this.setStartDate(options.startDate || this.element.data('date-startdate'));
		this.setEndDate(options.endDate || this.element.data('date-enddate'));
		this.setDaysOfWeekDisabled(options.daysOfWeekDisabled || this.element.data('date-days-of-week-disabled'));
		this.fillDow();
		this.fillMonths();
		this.setRange(options.range);

		this._allow_update = true;

		this.update();
		this.showMode();

		if (this.isInline) {
			this.show();
		}
	};

	Datepicker.prototype = {
		constructor: Datepicker,

		_events: [],
		_secondaryEvents: [],
		_applyEvents: function(evs) {
			for (var i = 0, el, ev; i < evs.length; i++) {
				el = evs[i][0];
				ev = evs[i][1];
				el.on(ev);
			}
		},
		_unapplyEvents: function(evs) {
			for (var i = 0, el, ev; i < evs.length; i++) {
				el = evs[i][0];
				ev = evs[i][1];
				el.off(ev);
			}
		},
		_buildEvents: function() {
			if (this.isInput) { // single input
				this._events = [
					[this.element, {
						focus: $.proxy(this.show, this),
						keyup: $.proxy(this.update, this),
						keydown: $.proxy(this.keydown, this)
					}]
				];
			} else if (this.component && this.hasInput) { // component: input + button
				this._events = [
				// For components that are not readonly, allow keyboard nav
				[this.element.find('input'), {
					focus: $.proxy(this.show, this),
					keyup: $.proxy(this.update, this),
					keydown: $.proxy(this.keydown, this)
				}],
					[this.component, {
						click: $.proxy(this.show, this)
					}]
				];
			} else if (this.element.is('div')) { // inline datepicker
				this.isInline = true;
			} else {
				this._events = [
					[this.element, {
						click: $.proxy(this.show, this)
					}]
				];
			}

			this._secondaryEvents = [
				[this.picker, {
					click: $.proxy(this.click, this)
				}],
				[$(window), {
					resize: $.proxy(this.place, this)
				}],
				[$(document), {
					mousedown: $.proxy(function(e) {
						// Clicked outside the datepicker, hide it
						if ($(e.target).closest('.datepicker.datepicker-inline, .datepicker.datepicker-dropdown').length === 0) {
							this.hide();
						}
					}, this)
				}]
			];
		},
		_attachEvents: function() {
			this._detachEvents();
			this._applyEvents(this._events);
		},
		_detachEvents: function() {
			this._unapplyEvents(this._events);
		},
		_attachSecondaryEvents: function() {
			this._detachSecondaryEvents();
			this._applyEvents(this._secondaryEvents);
		},
		_detachSecondaryEvents: function() {
			this._unapplyEvents(this._secondaryEvents);
		},

		show: function(e) {
			if (!this.isInline) this.picker.appendTo('body');
			this.picker.show();
			this.height = this.component ? this.component.outerHeight() : this.element.outerHeight();
			this.place();
			this._attachSecondaryEvents();
			if (e) {
				e.preventDefault();
			}
			this.element.trigger({
				type: 'show',
				date: this.date
			});
		},

		hide: function(e) {
			if (this.isInline) return;
			if (!this.picker.is(':visible')) return;
			this.picker.hide().detach();
			this._detachSecondaryEvents();
			this.viewMode = this.startViewMode;
			this.showMode();

			if (
			this.forceParse && (
			this.isInput && this.element.val() || this.hasInput && this.element.find('input').val())) this.setValue();
			this.element.trigger({
				type: 'hide',
				date: this.date
			});
		},

		remove: function() {
			this.hide();
			this._detachEvents();
			this._detachSecondaryEvents();
			this.picker.remove();
			delete this.element.data().datepicker;
			if (!this.isInput) {
				delete this.element.data().date;
			}
		},

		getDate: function() {
			var d = this.getUTCDate();
			return new Date(d.getTime() + (d.getTimezoneOffset() * 60000));
		},

		getUTCDate: function() {
			return this.date;
		},

		setDate: function(d) {
			this.setUTCDate(new Date(d.getTime() - (d.getTimezoneOffset() * 60000)));
		},

		setUTCDate: function(d) {
			this.date = d;
			this.setValue();
		},

		setValue: function() {
			var formatted = this.getFormattedDate();
			if (!this.isInput) {
				if (this.component) {
					this.element.find('input').val(formatted);
				}
			} else {
				this.element.val(formatted);
			}
		},

		getFormattedDate: function(format) {
			if (format === undefined) format = this.format;
			return DPGlobal.formatDate(this.date, format, this.language);
		},

		setStartDate: function(startDate) {
			this.startDate = startDate || -Infinity;
			if (this.startDate !== -Infinity) {
				this.startDate = DPGlobal.parseDate(this.startDate, this.format, this.language);
			}
			this.update();
			this.updateNavArrows();
		},

		setEndDate: function(endDate) {
			this.endDate = endDate || Infinity;
			if (this.endDate !== Infinity) {
				this.endDate = DPGlobal.parseDate(this.endDate, this.format, this.language);
			}
			this.update();
			this.updateNavArrows();
		},

		setDaysOfWeekDisabled: function(daysOfWeekDisabled) {
			this.daysOfWeekDisabled = daysOfWeekDisabled || [];
			if (!$.isArray(this.daysOfWeekDisabled)) {
				this.daysOfWeekDisabled = this.daysOfWeekDisabled.split(/,\s*/);
			}
			this.daysOfWeekDisabled = $.map(this.daysOfWeekDisabled, function(d) {
				return parseInt(d, 10);
			});
			this.update();
			this.updateNavArrows();
		},

		place: function() {
			if (this.isInline) return;
			var zIndex = parseInt(this.element.parents().filter(function() {
				return $(this).css('z-index') != 'auto';
			}).first().css('z-index')) + 10;
			var offset = this.component ? this.component.parent().offset() : this.element.offset();
			var height = this.component ? this.component.outerHeight(true) : this.element.outerHeight(true);
			this.picker.css({
				top: offset.top + height + 9,
				left: offset.left,
				zIndex: zIndex
			});
		},

		_allow_update: true,
		update: function() {
			if (!this._allow_update) return;

			var date, fromArgs = false;
			if (arguments && arguments.length && (typeof arguments[0] === 'string' || arguments[0] instanceof Date)) {
				date = arguments[0];
				fromArgs = true;
			} else {
				date = this.isInput ? this.element.val() : this.element.data('date') || this.element.find('input').val();
				delete this.element.data().date;
			}

			this.date = DPGlobal.parseDate(date, this.format, this.language);

			if (fromArgs) this.setValue();

			if (this.date < this.startDate) {
				this.viewDate = new Date(this.startDate);
			} else if (this.date > this.endDate) {
				this.viewDate = new Date(this.endDate);
			} else {
				this.viewDate = new Date(this.date);
			}
			this.fill();
		},

		fillDow: function() {
			var dowCnt = this.weekStart,
				html = '<tr>';
			if (this.calendarWeeks) {
				var cell = '<th class="cw">&nbsp;</th>';
				html += cell;
				this.picker.find('.datepicker-days thead tr:first-child').prepend(cell);
			}
			while (dowCnt < this.weekStart + 7) {
				var clsName = (dowCnt > 5 || dowCnt === 0) ? "red" : "";
				html += '<th class="dow '+ clsName +'">' + dates[this.language].daysMin[(dowCnt++) % 7] + '</th>';
			}
			html += '</tr>';
			this.picker.find('.datepicker-days thead').append(html);
		},

		fillMonths: function() {
			var html = '',
				i = 0;
			while (i < 12) {
				html += '<span class="month">' + dates[this.language].monthsShort[i++] + '</span>';
			}
			this.picker.find('.datepicker-months td').html(html);
		},

		setRange: function(range) {
			if (!range || !range.length) delete this.range;
			else this.range = $.map(range, function(d) {
				return d.valueOf();
			});
			this.fill();
		},

		// edit by ghj 2013-04-10 23:05:08
		getClassNames: function(date, flag) {
			var cls = [],
				year = this.viewDate.getUTCFullYear(),
				month = flag ? this.viewDate.getUTCMonth() + 1 : this.viewDate.getUTCMonth(),
				currentDate = this.date.valueOf(),
				today = new Date();
			if (date.getUTCFullYear() < year || (date.getUTCFullYear() == year && date.getUTCMonth() < month)) {
				cls.push('old');
			}
            //changed by pl 2013-10-30
            if (date.getUTCFullYear() > today.getUTCFullYear() || date.getUTCFullYear() == today.getUTCFullYear() && date.getUTCMonth() > today.getUTCMonth() || date.getUTCFullYear() == year && date.getUTCMonth() == today.getUTCMonth() && date.getUTCDate() > today.getDate() || date.getUTCFullYear() > year && date.getUTCMonth() - month > -12) {
                cls.push('new disabled');
            }
			// Compare internal UTC date with local today, not UTC today
			if (this.todayHighlight && date.getUTCFullYear() == today.getFullYear() && date.getUTCMonth() == today.getMonth() && date.getUTCDate() == today.getDate()) {
				cls.push('today');
			}
			if (currentDate && date.valueOf() == currentDate) {
				cls.push('active');
			}
			if (date.valueOf() < this.startDate || date.valueOf() > this.endDate || $.inArray(date.getUTCDay(), this.daysOfWeekDisabled) !== -1) {
				cls.push('disabled');
			}
			if (this.range) {
				if (date > this.range[0] && date < this.range[this.range.length - 1]) {
					cls.push('range');
				}
				if ($.inArray(date.valueOf(), this.range) != -1) {
					cls.push('selected');
				}
			}
			return cls;
		},

		fill: function() {
			var d = new Date(this.viewDate),
				year = d.getUTCFullYear(),
				month = d.getUTCMonth(),
				startYear = this.startDate !== -Infinity ? this.startDate.getUTCFullYear() : -Infinity,
				startMonth = this.startDate !== -Infinity ? this.startDate.getUTCMonth() : -Infinity,
				endYear = this.endDate !== Infinity ? this.endDate.getUTCFullYear() : Infinity,
				endMonth = this.endDate !== Infinity ? this.endDate.getUTCMonth() : Infinity,
				currentDate = this.date && this.date.valueOf();
			// edit by ghj 2013-04-04 22:33:47
			/*this.picker.find('.datepicker-days thead th.datepicker-switch')
				.text(dates[this.language].months[month]+' '+year);*/
			var rMonth, rYear;
			if (month < 11) {
				rMonth = month + 1;
				rYear = year;
			} else {
				rMonth = 0;
				rYear = year + 1;
			}
			this.picker.find('.leftCalendar .datepicker-days thead th.datepicker-switch')
				.text(year + dates[this.language].year[0] + dates[this.language].months[month]);
			this.picker.find('.leftCalendar .month-num').text(month + 1);
			this.picker.find('.rightCalendar .datepicker-days thead th.datepicker-switch')
				.text(rYear + dates[this.language].year[0] + dates[this.language].months[rMonth]);
			this.picker.find('.rightCalendar .month-num').text(rMonth + 1);
			// show today text		
			var todayText = this.picker.find('.range-date .today');
			if (todayText.length === 0 && this.todayBtn !== false) {
				this.picker.find('.range-date')
					.prepend('<a class="today">' + dates[this.language].today + "</a>")
				//.toggle(this.todayBtn !== false);
			}
			var latestText = this.picker.find('.range-date .latest-date');
			if (latestText.length === 0 && typeof this.rangeDate === "function") {
				this.picker.find('.range-date')
					.append('<span class="latest-date"><a class="latestDay">今天</a><a class="latestWeek">最近一周</a><a class="latestMonth">最近一月</a><a class="latestQuarter">最近一季度</a></span>')
					.show();
			}			
			// end
			this.updateNavArrows();
			this.fillMonths();
			var prevMonth = UTCDate(year, month - 1, 28, 0, 0, 0, 0),
				day = DPGlobal.getDaysInMonth(prevMonth.getUTCFullYear(), prevMonth.getUTCMonth());
			prevMonth.setUTCDate(day);
			prevMonth.setUTCDate(day - (prevMonth.getUTCDay() - this.weekStart + 7) % 7);
			var nextMonth = new Date(prevMonth);
			nextMonth.setUTCDate(nextMonth.getUTCDate() + 42);
			nextMonth = nextMonth.valueOf();
			var html = [];
			var clsName;
			while (prevMonth.valueOf() < nextMonth) {
				if (prevMonth.getUTCDay() == this.weekStart) {
					html.push('<tr>');
					if (this.calendarWeeks) {
						// ISO 8601: First week contains first thursday.
						// ISO also states week starts on Monday, but we can be more abstract here.
						var
						// Start of current week: based on weekstart/current date
						ws = new Date(+prevMonth + (this.weekStart - prevMonth.getUTCDay() - 7) % 7 * 864e5),
							// Thursday of this week
							th = new Date(+ws + (7 + 4 - ws.getUTCDay()) % 7 * 864e5),
							// First Thursday of year, year from thursday
							yth = new Date(+(yth = UTCDate(th.getUTCFullYear(), 0, 1)) + (7 + 4 - yth.getUTCDay()) % 7 * 864e5),
							// Calendar week: ms between thursdays, div ms per day, div 7 days
							calWeek = (th - yth) / 864e5 / 7 + 1;
						html.push('<td class="cw">' + calWeek + '</td>');

					}
				}
				clsName = this.getClassNames(prevMonth);
				clsName.push('day');
				// edit by ghj 2013-06-29 20:55:58
				var tclsName = "";
				if (prevMonth.getUTCMonth() + 1 - month === 1) {						
					if (prevMonth.getDay() === 0) {
						tclsName = "red";
					} else if (this.weekStart === 0 && prevMonth.getDay() > 5) {
						tclsName = "red";
					} else if (this.weekStart === 1 && prevMonth.getDay() > 5) {
						tclsName = "red";
					}
				}
				html.push('<td class="' + clsName.join(' ') + ' '+ tclsName + '">' + prevMonth.getUTCDate() + '</td>');
				if (prevMonth.getUTCDay() == this.weekEnd) {
					html.push('</tr>');
				}
				prevMonth.setUTCDate(prevMonth.getUTCDate() + 1);
			}
			// edit by ghj 2013-04-04 22:31:48
			//this.picker.find('.datepicker-days tbody').empty().append(html.join(''));
			this.picker.find('.leftCalendar .datepicker-days tbody').html(html.join(''));

			// right calendar start by ghj 2013-04-10 22:17:06
			var currentMonth = UTCDate(year, month, 28, 0, 0, 0, 0),
				day = DPGlobal.getDaysInMonth(currentMonth.getUTCFullYear(), currentMonth.getUTCMonth());
			currentMonth.setUTCDate(day);
			currentMonth.setUTCDate(day - (currentMonth.getUTCDay() - this.weekStart + 7) % 7);
			var rightNextMonth = new Date(currentMonth);
			// edit by ghj 2013-04-04 20:19:58
			rightNextMonth.setUTCDate(rightNextMonth.getUTCDate() + 42);
			rightNextMonth = rightNextMonth.valueOf();
			var rhtml = [];
			var rightClsName;
			while (currentMonth.valueOf() < rightNextMonth) {
				if (currentMonth.getUTCDay() == this.weekStart) {
					rhtml.push('<tr>');
					if (this.calendarWeeks) {
						// ISO 8601: First week contains first thursday.
						// ISO also states week starts on Monday, but we can be more abstract here.
						var
						// Start of current week: based on weekstart/current date
						ws = new Date(+currentMonth + (this.weekStart - currentMonth.getUTCDay() - 7) % 7 * 864e5),
							// Thursday of this week
							th = new Date(+ws + (7 + 4 - ws.getUTCDay()) % 7 * 864e5),
							// First Thursday of year, year from thursday
							yth = new Date(+(yth = UTCDate(th.getUTCFullYear(), 0, 1)) + (7 + 4 - yth.getUTCDay()) % 7 * 864e5),
							// Calendar week: ms between thursdays, div ms per day, div 7 days
							calWeek = (th - yth) / 864e5 / 7 + 1;
						rhtml.push('<td class="cw">' + calWeek + '</td>');

					}
				}
				rightClsName = this.getClassNames(currentMonth, true);
				rightClsName.push('day');
				// edit by ghj 2013-06-29 20:55:58
				var tclsName = "";
				if (currentMonth.getUTCMonth() + 1 - prevMonth.getUTCMonth() === 1) {						
					if (currentMonth.getDay() === 0) {
						tclsName = "red";
					} else if (this.weekStart === 0 && currentMonth.getDay() > 5) {
						tclsName = "red";
					} else if (this.weekStart === 1 && currentMonth.getDay() > 5) {
						tclsName = "red";
					}
				}
				rhtml.push('<td class="' + rightClsName.join(' ') + ' '+ tclsName + '">' + currentMonth.getUTCDate() + '</td>');
				if (currentMonth.getUTCDay() == this.weekEnd) {
					html.push('</tr>');
				}
				currentMonth.setUTCDate(currentMonth.getUTCDate() + 1);
			}
			this.picker.find('.rightCalendar .datepicker-days tbody').html($(rhtml.join('')));
			// right calendar end
			var currentYear = this.date && this.date.getUTCFullYear();

			var months = this.picker.find('.leftCalendar .datepicker-months')
				.find('th:eq(1)')
				.text(year + dates[this.language].year[0])
				.end()
				.find('span').removeClass('active');
			if (currentYear && currentYear == year) {
				months.eq(this.date.getUTCMonth()).addClass('active');
			}
			if (year < startYear || year > endYear) {
				months.addClass('disabled');
			}
			if (year == startYear) {
				months.slice(0, startMonth).addClass('disabled');
			}
			if (year == endYear) {
				months.slice(endMonth + 1).addClass('disabled');
			}
			// add by ghj 2013-04-09 21:15:01
			var rightMonths = this.picker.find('.rightCalendar .datepicker-months')
				.find('th:eq(1)')
				.text((year + 1) + dates[this.language].year[0])
				.end()
				.find('span').removeClass('active');

			if (currentYear && currentYear == year + 1) {
				rightMonths.eq(this.date.getUTCMonth()).addClass('active');
			}
			if (year + 1 < startYear || year + 1 > endYear) {
				rightMonths.addClass('disabled');
			}
			if (year + 1 == startYear) {
				rightMonths.slice(0, startMonth).addClass('disabled');
			}
			if (year + 1 == endYear) {
				rightMonths.slice(endMonth + 1).addClass('disabled');
			}
			// end
			html = '';
			year = parseInt(year / 10, 10) * 10;
			var yearCont = this.picker.find('.leftCalendar .datepicker-years')
				.find('th:eq(1)')
				.text(year + '-' + (year + 9) + dates[this.language].year[0])
				.end()
				.find('td');
			year -= 1;
			var today = new Date(),
				thisyear = today.getUTCFullYear();
			for (var i = -1; i < 11; i++) {
				//html += '<span class="year' + (i == -1 || i == 10 ? ' old' : '') + (currentYear == year ? ' active' : '') + (year < startYear || year > endYear ? ' disabled' : '') + '">' + year + '</span>';
				html += '<span class="year' + (i == -1 || i == 10 ? ' old' : '') + (currentYear == year ? ' active' : '') + (year > thisyear ? ' disabled' : '') + '">' + year + '</span>';				
				year += 1;
			}
			yearCont.html(html);

			// add by ghj 2013-04-09 21:56:20
			rHtml = '';
			rYear = parseInt(year / 10, 10) * 10;
			var rYearCont = this.picker.find('.rightCalendar .datepicker-years')
				.find('th:eq(1)')
				.text(rYear + '-' + (rYear + 9) + dates[this.language].year[0])
				.end()
				.find('td');
			rYear -= 1;
			for (var i = -1; i < 11; i++) {
				//rHtml += '<span class="year' + (i == -1 || i == 10 ? ' old' : '') + (currentYear == rYear ? ' active' : '') + (rYear < startYear || rYear > endYear ? ' disabled' : '') + '">' + rYear + '</span>';
				rHtml += '<span class="year' + (i == -1 || i == 10 ? ' old' : '') + (currentYear == rYear ? ' active' : '') + (rYear > thisyear ? ' disabled' : '') + '">' + rYear + '</span>';
				rYear += 1;
			}
			rYearCont.html(rHtml);
			// end
		},

		updateNavArrows: function() {
			if (!this._allow_update) return;

			var d = new Date(this.viewDate),
				year = d.getUTCFullYear(),
				month = d.getUTCMonth();
			switch (this.viewMode) {
				case 0:
					if (this.startDate !== -Infinity && year <= this.startDate.getUTCFullYear() && month <= this.startDate.getUTCMonth()) {
						this.picker.find('.prev').css({
							visibility: 'hidden'
						});
					} else {
						this.picker.find('.prev').css({
							visibility: 'visible'
						});
					}
					if (this.endDate !== Infinity && year >= this.endDate.getUTCFullYear() && month >= this.endDate.getUTCMonth()) {
						this.picker.find('.next').css({
							visibility: 'hidden'
						});
					} else {
						this.picker.find('.next').css({
							visibility: 'visible'
						});
					}
					break;
				case 1:
				case 2:
					if (this.startDate !== -Infinity && year <= this.startDate.getUTCFullYear()) {
						this.picker.find('.prev').css({
							visibility: 'hidden'
						});
					} else {
						this.picker.find('.prev').css({
							visibility: 'visible'
						});
					}
					if (this.endDate !== Infinity && year >= this.endDate.getUTCFullYear()) {
						this.picker.find('.next').css({
							visibility: 'hidden'
						});
					} else {
						this.picker.find('.next').css({
							visibility: 'visible'
						});
					}
					break;
			}
		},

		click: function(e) {
			e.preventDefault();
			var target = $(e.target).closest('span, td, th, a');
			if (target.length == 1) {
				switch (target[0].nodeName.toLowerCase()) {
					case 'th':
						switch (target[0].className) {
							case 'datepicker-switch':
								this.showMode(1);
								break;
							case 'prev':
							case 'next':
								var dir = DPGlobal.modes[this.viewMode].navStep * (target[0].className == 'prev' ? -1 : 1);
								switch (this.viewMode) {
									case 0:
										this.viewDate = this.moveMonth(this.viewDate, dir);
										break;
									case 1:
									case 2:
										this.viewDate = this.moveYear(this.viewDate, dir);
										break;
								}
								this.fill();
								break;
						}
						break;
					case 'span':
						if (!target.is('.disabled')) {
							this.viewDate.setUTCDate(1);
							if (target.is('.month')) {
								var day = 1;
								var month = target.parent().find('span').index(target);
								// edit by ghj 2013-04-09 21:50:54
								var len = target.parents(".rightCalendar").length;
								if (len === 1) {
									var year = this.viewDate.getUTCFullYear() + 1;
								} else {
									var year = this.viewDate.getUTCFullYear();
								}
								this.viewDate.setUTCFullYear(year);
								// end
								this.viewDate.setUTCMonth(month);
								this.element.trigger({
									type: 'changeMonth',
									date: this.viewDate
								});
								if (this.minViewMode == 1) {
									this._setDate(UTCDate(year, month, day, 0, 0, 0, 0));
								}
							} else {
								var year = parseInt(target.text(), 10) || 0;
								var day = 1;
								var month = 0;
								this.viewDate.setUTCFullYear(year);
								this.element.trigger({
									type: 'changeYear',
									date: this.viewDate
								});
								if (this.minViewMode == 2) {
									this._setDate(UTCDate(year, month, day, 0, 0, 0, 0));
								}
							}
							this.showMode(-1);
							this.fill();
						}
						break;
					case 'td':
						if (target.is('.day') && !target.is('.disabled')) {
							var day = parseInt(target.text(), 10) || 1;
							var year = this.viewDate.getUTCFullYear();
							// add by ghj 2013-04-09 21:21:55
							var len = target.parents(".rightCalendar").length;
							if (len === 1) {
								var month = this.viewDate.getUTCMonth() + 1;
							} else {
								var month = this.viewDate.getUTCMonth();
							}
							if (target.is('.old')) {
								if (month === 0) {
									month = 11;
									year -= 1;
								} else {
									month -= 1;
								}
							} else if (target.is('.new')) {
								if (month == 11) {
									month = 0;
									year += 1;
								} else {
									month += 1;
								}
							}
							if (typeof(this.stepDate) === "object" && this.stepDate.type !== "") {
								var yearNum = 0,
									monthNum = 0;
								if (this.stepDate.type === "month") {
									yearNum = 0;
									monthNum = 1;
								} else if(this.stepDate.type === "quarter") {									
									yearNum = 0;
									monthNum = 3;
								} else if (this.stepDate.type === "year") {									
									yearNum = 1;
									monthNum = 0;
								}
								var date = UTCDate(year, month, day, 0, 0, 0, 0);
								var sDate = UTCDate(date.getFullYear(), date.getMonth(), date.getDate(), 0, 0, 0),
									eDate = UTCDate(date.getFullYear() + yearNum, date.getMonth() + monthNum, date.getDate()-1, 0, 0, 0);
								var eDateBig = UTCDate(date.getFullYear() + yearNum, date.getMonth() + monthNum +1, 0, 0, 0, 0).getDate();
								if (UTCDate(date.getFullYear() + yearNum, date.getMonth() + monthNum +1, 0, 0, 0, 0).getDate() == 30 && eDate.getDate() == 30) {
									eDate = UTCDate(date.getFullYear() + yearNum, date.getMonth() + monthNum, 29, 0, 0, 0);
								}
								if((date.getMonth() + monthNum) == 1 && ((eDate.getMonth() - sDate.getMonth()) == 2 || eDate.getDate() >= (eDateBig - 1))){
									if(sDate.getDate() > 1){
										eDate = UTCDate(date.getFullYear() + yearNum, date.getMonth() + monthNum, eDateBig - 1, 0, 0, 0);
									}
								}
								var startDate = DPGlobal.formatDate(sDate, this.format, this.language),
									endDate = DPGlobal.formatDate(eDate, this.format, this.language);
								this.stepDate.callback.call(this, startDate, endDate);
							} else {
								this._setDate(UTCDate(year, month, day, 0, 0, 0, 0));
							}
						}
						break;
						// add by ghj 2013-04-06 21:41:18
					case 'a':
						switch (target[0].className) {
							case 'today':
								var date = new Date();
								date = UTCDate(date.getFullYear(), date.getMonth(), date.getDate(), 0, 0, 0);

								this.showMode(-2);
								var which = this.todayBtn == 'linked' ? null : 'view';
								this._setDate(date, which);
								break;
							case 'latestDay':
								var date = new Date();
								var sDate = UTCDate(date.getFullYear(), date.getMonth(), date.getDate(), 0, 0, 0),
									eDate = date;
								var startDate = DPGlobal.formatDate(sDate, this.format, this.language),
									endDate = DPGlobal.formatDate(eDate, this.format, this.language);
								if (typeof this.rangeDate === "function") {
									this.rangeDate.call(this, startDate, endDate);
								}
								break;
							case 'latestWeek':
								var date = new Date();
								var sDate = UTCDate(date.getFullYear(), date.getMonth(), date.getDate() - 6, 0, 0, 0),
									eDate = date;
								var startDate = DPGlobal.formatDate(sDate, this.format, this.language),
									endDate = DPGlobal.formatDate(eDate, this.format, this.language);
								if (typeof this.rangeDate === "function") {
									this.rangeDate.call(this, startDate, endDate);
								}
								break;
							case 'latestMonth':
								var date = new Date();
								var sDate = UTCDate(date.getFullYear(), date.getMonth() - 1, date.getDate() + 1, 0, 0, 0),
									eDate = date;
								if (sDate.getDate() < eDate.getDate()) {
									sDate = UTCDate(date.getFullYear(), date.getMonth(), 1, 0, 0, 0);
								}									
								var startDate = DPGlobal.formatDate(sDate, this.format, this.language),
									endDate = DPGlobal.formatDate(eDate, this.format, this.language);
								if (typeof this.rangeDate === "function") {
									this.rangeDate.call(this, startDate, endDate);
								}
								break;
							case 'latestQuarter':
								var date = new Date();
								var sDate = UTCDate(date.getFullYear(), date.getMonth() - 3, date.getDate() + 1, 0, 0, 0),
									eDate = date;
								if (sDate.getDate() < eDate.getDate()) {
									sDate = UTCDate(date.getFullYear(), date.getMonth() - 2, 1, 0, 0, 0);
								}									
								var startDate = DPGlobal.formatDate(sDate, this.format, this.language),
									endDate = DPGlobal.formatDate(eDate, this.format, this.language);
								if (typeof this.rangeDate === "function") {
									this.rangeDate.call(this, startDate, endDate);
								}
								break;
						}
						// end
				}
			}
		},

		_setDate: function(date, which) {
			if (!which || which == 'date') this.date = date;
			if (!which || which == 'view') this.viewDate = date;
			this.fill();
			this.setValue();
			this.element.trigger({
				type: 'changeDate',
				date: this.date
			});
			var element;
			if (this.isInput) {
				element = this.element;
			} else if (this.component) {
				element = this.element.find('input');
			}
			if (element) {
				element.change();
				if (this.autoclose && (!which || which == 'date')) {
					this.hide();
				}
			}
		},

		moveMonth: function(date, dir) {
			if (!dir) return date;
			var new_date = new Date(date.valueOf()),
				day = new_date.getUTCDate(),
				month = new_date.getUTCMonth(),
				mag = Math.abs(dir),
				new_month, test;
			dir = dir > 0 ? 1 : -1;
			if (mag == 1) {
				test = dir == -1
				// If going back one month, make sure month is not current month
				// (eg, Mar 31 -> Feb 31 == Feb 28, not Mar 02)
				?


				function() {
					return new_date.getUTCMonth() == month;
				}
				// If going forward one month, make sure month is as expected
				// (eg, Jan 31 -> Feb 31 == Feb 28, not Mar 02)
				:


				function() {
					return new_date.getUTCMonth() != new_month;
				};
				new_month = month + dir;
				new_date.setUTCMonth(new_month);
				// Dec -> Jan (12) or Jan -> Dec (-1) -- limit expected date to 0-11
				if (new_month < 0 || new_month > 11) new_month = (new_month + 12) % 12;
			} else {
				// For magnitudes >1, move one month at a time...
				for (var i = 0; i < mag; i++)
				// ...which might decrease the day (eg, Jan 31 to Feb 28, etc)...
				new_date = this.moveMonth(new_date, dir);
				// ...then reset the day, keeping it in the new month
				new_month = new_date.getUTCMonth();
				new_date.setUTCDate(day);
				test = function() {
					return new_month != new_date.getUTCMonth();
				};
			}
			// Common date-resetting loop -- if date is beyond end of month, make it
			// end of month
			while (test()) {
				new_date.setUTCDate(--day);
				new_date.setUTCMonth(new_month);
			}
			return new_date;
		},

		moveYear: function(date, dir) {
			return this.moveMonth(date, dir * 12);
		},

		dateWithinRange: function(date) {
			return date >= this.startDate && date <= this.endDate;
		},

		keydown: function(e) {
			if (this.picker.is(':not(:visible)')) {
				if (e.keyCode == 27) // allow escape to hide and re-show picker
				this.show();
				return;
			}
			var dateChanged = false,
				dir, day, month,
				newDate, newViewDate;
			switch (e.keyCode) {
				case 27:
					// escape
					this.hide();
					e.preventDefault();
					break;
				case 37:
					// left
				case 39:
					// right
					if (!this.keyboardNavigation) break;
					dir = e.keyCode == 37 ? -1 : 1;
					if (e.ctrlKey) {
						newDate = this.moveYear(this.date, dir);
						newViewDate = this.moveYear(this.viewDate, dir);
					} else if (e.shiftKey) {
						newDate = this.moveMonth(this.date, dir);
						newViewDate = this.moveMonth(this.viewDate, dir);
					} else {
						newDate = new Date(this.date);
						newDate.setUTCDate(this.date.getUTCDate() + dir);
						newViewDate = new Date(this.viewDate);
						newViewDate.setUTCDate(this.viewDate.getUTCDate() + dir);
					}
					if (this.dateWithinRange(newDate)) {
						this.date = newDate;
						this.viewDate = newViewDate;
						this.setValue();
						this.update();
						e.preventDefault();
						dateChanged = true;
					}
					break;
				case 38:
					// up
				case 40:
					// down
					if (!this.keyboardNavigation) break;
					dir = e.keyCode == 38 ? -1 : 1;
					if (e.ctrlKey) {
						newDate = this.moveYear(this.date, dir);
						newViewDate = this.moveYear(this.viewDate, dir);
					} else if (e.shiftKey) {
						newDate = this.moveMonth(this.date, dir);
						newViewDate = this.moveMonth(this.viewDate, dir);
					} else {
						newDate = new Date(this.date);
						newDate.setUTCDate(this.date.getUTCDate() + dir * 7);
						newViewDate = new Date(this.viewDate);
						newViewDate.setUTCDate(this.viewDate.getUTCDate() + dir * 7);
					}
					if (this.dateWithinRange(newDate)) {
						this.date = newDate;
						this.viewDate = newViewDate;
						this.setValue();
						this.update();
						e.preventDefault();
						dateChanged = true;
					}
					break;
				case 13:
					// enter
					this.hide();
					e.preventDefault();
					break;
				case 9:
					// tab
					this.hide();
					break;
			}
			if (dateChanged) {
				this.element.trigger({
					type: 'changeDate',
					date: this.date
				});
				var element;
				if (this.isInput) {
					element = this.element;
				} else if (this.component) {
					element = this.element.find('input');
				}
				if (element) {
					element.change();
				}
			}
		},

		showMode: function(dir) {
			if (dir) {
				this.viewMode = Math.max(this.minViewMode, Math.min(2, this.viewMode + dir));
			}
			/*
				vitalets: fixing bug of very special conditions:
				jquery 1.7.1 + webkit + show inline datepicker in bootstrap popover.
				Method show() does not set display css correctly and datepicker is not shown.
				Changed to .css('display', 'block') solve the problem.
				See https://github.com/vitalets/x-editable/issues/37

				In jquery 1.7.2+ everything works fine.
			*/
			// edit by ghj 2013-04-09 22:46:07
			//this.picker.find('>div').hide().filter('.datepicker-'+DPGlobal.modes[this.viewMode].clsName).show();
			this.picker.find('>div>div').hide().filter('.datepicker-' + DPGlobal.modes[this.viewMode].clsName).css('display', 'block');
			this.updateNavArrows();
		}
	};

	var DateRangePicker = function(element, options) {
		this.element = $(element);
		this.inputs = $.map(options.inputs, function(i) {
			return i.jquery ? i[0] : i;
		});
		delete options.inputs;

		$(this.inputs)
			.datepicker(options)
			.bind('changeDate', $.proxy(this.dateUpdated, this));

		this.pickers = $.map(this.inputs, function(i) {
			return $(i).data('datepicker');
		});
		this.updateDates();
	};
	DateRangePicker.prototype = {
		updateDates: function() {
			this.dates = $.map(this.pickers, function(i) {
				return i.date;
			});
			this.updateRanges();
		},
		updateRanges: function() {
			var range = $.map(this.dates, function(d) {
				return d.valueOf();
			});
			$.each(this.pickers, function(i, p) {
				p.setRange(range);
			});
		},
		dateUpdated: function(e) {
			var dp = $(e.target).data('datepicker'),
				new_date = e.date,
				i = $.inArray(e.target, this.inputs),
				l = this.inputs.length;
			if (i == -1) return;

			if (new_date < this.dates[i]) {
				// Date being moved earlier/left
				while (i >= 0 && new_date < this.dates[i]) {
					this.pickers[i--].setUTCDate(new_date);
				}
			} else if (new_date > this.dates[i]) {
				// Date being moved later/right
				while (i < l && new_date > this.dates[i]) {
					this.pickers[i++].setUTCDate(new_date);
				}
			}
			this.updateDates();
		},
		remove: function() {
			$.map(this.pickers, function(p) {
				p.remove();
			});
			delete this.element.data().datepicker;
		}
	};

	var old = $.fn.datepicker;
	$.fn.datepicker = function(option) {
		var args = Array.apply(null, arguments);
		args.shift();
		return this.each(function() {
			var $this = $(this),
				data = $this.data('datepicker'),
				options = typeof option == 'object' && option;
			if (!data) {
				if ($this.is('.input-daterange') || options.inputs) {
					var opts = {
						inputs: options.inputs || $this.find('input').toArray()
					};
					$this.data('datepicker', (data = new DateRangePicker(this, $.extend(opts, $.fn.datepicker.defaults, options))));
				} else {
					$this.data('datepicker', (data = new Datepicker(this, $.extend({}, $.fn.datepicker.defaults, options))));
				}
			}
			if (typeof option == 'string' && typeof data[option] == 'function') {
				data[option].apply(data, args);
			}
		});
	};

	$.fn.datepicker.defaults = {};
	$.fn.datepicker.Constructor = Datepicker;
	var dates = $.fn.datepicker.dates = {
		cn: {
			days: ["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期日"],
			daysShort: ["周日", "周一", "周二", "周三", "周四", "周五", "周六", "周日"],
			daysMin: ["日", "一", "二", "三", "四", "五", "六", "日"],
			months: ["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"],
			monthsShort: ["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"],
			year: ["年"],
			today: ""
		}
	};

	var DPGlobal = {
		modes: [{
			clsName: 'days',
			navFnc: 'Month',
			navStep: 1
		}, {
			clsName: 'months',
			navFnc: 'FullYear',
			navStep: 1
		}, {
			clsName: 'years',
			navFnc: 'FullYear',
			navStep: 10
		}],
		isLeapYear: function(year) {
			return (((year % 4 === 0) && (year % 100 !== 0)) || (year % 400 === 0));
		},
		getDaysInMonth: function(year, month) {
			return [31, (DPGlobal.isLeapYear(year) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][month];
		},
		validParts: /dd?|DD?|mm?|MM?|yy(?:yy)?/g,
		nonpunctuation: /[^ -\/:-@\[\u3400-\u9fff-`{-~\t\n\r]+/g,
		parseFormat: function(format) {
			// IE treats \0 as a string end in inputs (truncating the value),
			// so it's a bad format delimiter, anyway
			var separators = format.replace(this.validParts, '\0').split('\0'),
				parts = format.match(this.validParts);
			if (!separators || !separators.length || !parts || parts.length === 0) {
				throw new Error("Invalid date format.");
			}
			return {
				separators: separators,
				parts: parts
			};
		},
		parseDate: function(date, format, language) {
			if (date instanceof Date) return date;
			if (/^[\-+]\d+[dmwy]([\s,]+[\-+]\d+[dmwy])*$/.test(date)) {
				var part_re = /([\-+]\d+)([dmwy])/,
					parts = date.match(/([\-+]\d+)([dmwy])/g),
					part, dir;
				date = new Date();
				for (var i = 0; i < parts.length; i++) {
					part = part_re.exec(parts[i]);
					dir = parseInt(part[1]);
					switch (part[2]) {
						case 'd':
							date.setUTCDate(date.getUTCDate() + dir);
							break;
						case 'm':
							date = Datepicker.prototype.moveMonth.call(Datepicker.prototype, date, dir);
							break;
						case 'w':
							date.setUTCDate(date.getUTCDate() + dir * 7);
							break;
						case 'y':
							date = Datepicker.prototype.moveYear.call(Datepicker.prototype, date, dir);
							break;
					}
				}
				return UTCDate(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate(), 0, 0, 0);
			}
			var parts = date && date.match(this.nonpunctuation) || [],
				date = new Date(),
				parsed = {},
				setters_order = ['yyyy', 'yy', 'M', 'MM', 'm', 'mm', 'd', 'dd'],
				setters_map = {
					yyyy: function(d, v) {
						return d.setUTCFullYear(v);
					},
					yy: function(d, v) {
						return d.setUTCFullYear(2000 + v);
					},
					m: function(d, v) {
						v -= 1;
						while (v < 0) v += 12;
						v %= 12;
						d.setUTCMonth(v);
						while (d.getUTCMonth() != v)
						d.setUTCDate(d.getUTCDate() - 1);
						return d;
					},
					d: function(d, v) {
						return d.setUTCDate(v);
					}
				},
				val, filtered, part;
			setters_map['M'] = setters_map['MM'] = setters_map['mm'] = setters_map['m'];
			setters_map['dd'] = setters_map['d'];
			date = UTCDate(date.getFullYear(), date.getMonth(), date.getDate(), 0, 0, 0);
			var fparts = format.parts.slice();
			// Remove noop parts
			if (parts.length != fparts.length) {
				fparts = $(fparts).filter(function(i, p) {
					return $.inArray(p, setters_order) !== -1;
				}).toArray();
			}
			// Process remainder
			if (parts.length == fparts.length) {
				for (var i = 0, cnt = fparts.length; i < cnt; i++) {
					val = parseInt(parts[i], 10);
					part = fparts[i];
					if (isNaN(val)) {
						switch (part) {
							case 'MM':
								filtered = $(dates[language].months).filter(function() {
									var m = this.slice(0, parts[i].length),
										p = parts[i].slice(0, m.length);
									return m == p;
								});
								val = $.inArray(filtered[0], dates[language].months) + 1;
								break;
							case 'M':
								filtered = $(dates[language].monthsShort).filter(function() {
									var m = this.slice(0, parts[i].length),
										p = parts[i].slice(0, m.length);
									return m == p;
								});
								val = $.inArray(filtered[0], dates[language].monthsShort) + 1;
								break;
						}
					}
					parsed[part] = val;
				}
				for (var i = 0, s; i < setters_order.length; i++) {
					s = setters_order[i];
					if (s in parsed && !isNaN(parsed[s])) setters_map[s](date, parsed[s]);
				}
			}
			return date;
		},
		formatDate: function(date, format, language) {
			var val = {
				d: date.getUTCDate(),
				D: dates[language].daysShort[date.getUTCDay()],
				DD: dates[language].days[date.getUTCDay()],
				m: date.getUTCMonth() + 1,
				M: dates[language].monthsShort[date.getUTCMonth()],
				MM: dates[language].months[date.getUTCMonth()],
				yy: date.getUTCFullYear().toString().substring(2),
				yyyy: date.getUTCFullYear()
			};
			val.dd = (val.d < 10 ? '0' : '') + val.d;
			val.mm = (val.m < 10 ? '0' : '') + val.m;
			var date = [],
				seps = $.extend([], format.separators);
			for (var i = 0, cnt = format.parts.length; i < cnt; i++) {
				if (seps.length) date.push(seps.shift());
				date.push(val[format.parts[i]]);
			}
			return date.join('');
		},
		headTemplate: '<thead>' + 
						'<tr>' + 
							'<th class="prev"><i class="icon-arrow-left"/></th>' + 
							'<th colspan="5" class="datepicker-switch"></th>' + 
							'<th class="next"><i class="icon-arrow-right"/></th>' + 
						'</tr>' + 
					 '</thead>',
		contTemplate: '<tbody><tr><td colspan="7"></td></tr></tbody>',
		footTemplate: '<tfoot><tr><th colspan="7" class="today"></th></tr></tfoot>',
		rangeDateTemplate: '<div class="range-date" style="display:none;"></div>'
	};
	DPGlobal.template = '<div class="datepicker">' +
							'<div class="calendar leftCalendar">' +
								'<div class="datepicker-days">' +
									'<div class="month-num"></div>' +
									'<table class=" table-condensed">' + 
										DPGlobal.headTemplate +
										'<tbody></tbody>' +
									'</table>' +
								'</div>' +
								'<div class="datepicker-months">' +
									'<table class="table-condensed">' + 
										DPGlobal.headTemplate + 
										DPGlobal.contTemplate +
									'</table>' +
								'</div>' +
								'<div class="datepicker-years">' +
									'<table class="table-condensed">' + 
										DPGlobal.headTemplate + 
										DPGlobal.contTemplate +
									'</table>' +
								'</div>' +
							'</div>' +
							'<div class="calendar rightCalendar">' +
								'<div class="datepicker-days">' +
									'<div class="month-num"></div>' +
									'<table class=" table-condensed">' + 
										DPGlobal.headTemplate +
										'<tbody></tbody>' +
									'</table>' +
								'</div>' +
								'<div class="datepicker-months">' +
									'<table class="table-condensed">' + 
										DPGlobal.headTemplate + 
										DPGlobal.contTemplate +
									'</table>' +
								'</div>' +
								'<div class="datepicker-years">' +
									'<table class="table-condensed">' + 
										DPGlobal.headTemplate + 
										DPGlobal.contTemplate +
									'</table>' +
								'</div>' +
							'</div>' + 
							DPGlobal.rangeDateTemplate +
						'</div>';

	$.fn.datepicker.DPGlobal = DPGlobal;


	/* DATEPICKER NO CONFLICT
	 * =================== */

	$.fn.datepicker.noConflict = function() {
		$.fn.datepicker = old;
		return this;
	};


	/* DATEPICKER DATA-API
	 * ================== */

	$(document).on(
		'focus.datepicker.data-api click.datepicker.data-api',
		'[data-provide="datepicker"]',

	function(e) {
		var $this = $(this);
		if ($this.data('datepicker')) return;
		e.preventDefault();
		// component click requires us to explicitly show it
		$this.datepicker('show');
	});
	$(function() {
		$('[data-provide="datepicker-inline"]').datepicker();
	});

}(window.jQuery);