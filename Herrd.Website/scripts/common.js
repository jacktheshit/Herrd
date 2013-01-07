/*
* List v1
*
*/

var AppSettings = {
	DEBUGMODE: true
};

var Main = {

	run: function () {
		Listing.item_add();
		Listing.item_archive();
		Listing.item_restore();
		Listing.item_delete();
		Listing.item_edit();
		Interact.hover();
		Interact.expand();
		Search.init();
		Account.dropdown();
		Input.inputKeypress();
		Input.inputClear();
		Utils.externalLink();
	}

};

var Listing = {

	item_add: function () {
		$('#form_add').submit(function (event) {
			event.preventDefault();
			var $form = $(this),
				$term = $form.find('textarea[name="Term"]').val(),
				$title = $form.find('input[name="Title"]').val(),
				url = $form.attr('action');

			$form.append('<div class="saved" style="opacity:0"><p>Saving...</p></div>');
			$('.saved', $form).animate({ opacity: 1 }, 300);

			$.ajax({
				type: "POST",
				url: url,
				data: { term: $term, title: $title },
				success: function (data) {
					$form.find('textarea[name="term"]').attr('value', '');
					$form.find('input[name="title"]').attr('value', '');
					$form.find($(Input.resetClass)).hide();
					$('.saved', $form).animate({ opacity: 0 }, 300, function () {
						//$('.arc_res p', $item).html('Saved');
					});
					var $updatedData = $(data).filter('div');
					$('.item_container').prepend($updatedData);
					$updatedData.animate({ opacity: 0 }, 0, function () {
						$(this).delay(300).animate({ opacity: 1 }, 300);
					});
				},
				error: function (xhr) {
					Debug.log(xhr.statusText);
					$('.saved').html("<p>Error : " + xhr.statusText + "</p>");
					$('.saved', $form).animate({ opacity: 0 }, 300);
				}
			});
		});
	},

	item_archive: function () {
		$('.btn_archive').live('click', function (event) {
			event.preventDefault();
			var $this = $(this),
				$item = $this.parents('.item'),
				itemId = $item.attr('id').split('item-')[1],
				url = $this.attr('href');

			$item.find('.controls_container').append('<div class="arc_res" style="opacity:0"><p>Archiving...</p></div>');
			$('.arc_res', $item).animate({ opacity: 1 }, 300);

			$.ajax({
				type: "POST",
				url: url,
				data: { id: itemId },
				success: function () {
					$('.arc_res', $item).delay(300).animate({ opacity: 0 }, 300, function () {
						//$('.arc_res p', $item).html('Archived');
						Listing.item_remove($item);
					});
				}
			});
		});
	},

	item_restore: function () {
		$('.btn_restore').click(function (event) {
			event.preventDefault();
			var $this = $(this),
				$item = $this.parents('.item'),
				$itemId = $item.attr('id').split('item-')[1],
				url = $this.attr('href');

			$item.find('.controls_container').append('<div class="arc_res" style="opacity:0"><p>Restoring...</p></div>');
			$('.arc_res', $item).animate({ opacity: 1 }, 300);

			$.ajax({
				type: "POST",
				url: url,
				data: { id: $itemId },
				success: function () {
					$('.arc_res', $item).delay(300).animate({ opacity: 0 }, 300, function () {
						//$('.arc_res p', $item).html('Restored');
						Listing.item_remove($item);
					});
				}
			});
		});
	},

	item_delete: function () {
		$('.btn_delete').live('click', function (event) {
			event.preventDefault();
			var $this = $(this),
				$item = $this.parents('.item'),
				$itemId = $item.attr('id').split('item-')[1],
				url = $this.attr('href');

			var confirmed = confirm("Are you sure you want to delete this item?");

			if (confirmed) {
				$item.find('.controls_container').append('<div class="delete" style="opacity:0"><p>Deleting...</p></div>');
				$('.delete', $item).animate({ opacity: 1 }, 300);

				$.ajax({
					type: "POST",
					url: url,
					data: { id: $itemId },
					success: function() {
						$('.delete', $item).delay(300).animate({ opacity: 0 }, 300, function() {
							//$('.arc_res p', $item).html('Deleted');
							Listing.item_remove($item);
						});
					}
				});
			}
		});
	},

	item_edit: function () {
		$('.btn_edit').live('click', function (event) {
			event.preventDefault();
			var $this = $(this),
				$item = $this.parents('.item');
			if ($this.hasClass('open')) {
				$this.removeClass('open');
				$('.form_container', $item).slideToggle(300, function () {
					$('span', $this).toggle();
				});
			} else {
				$('span', $this).toggle();
				if ($item.find(Input.inputClass).val().length > 0) {
					$(Input.resetClass, $item).show();
				};
				$('.form_container', $item).slideToggle(300, function () {
					$this.addClass('open');
				});
			};
		});

		$('.btn_cancel').live('click', function (event) {
			event.preventDefault();
			var $this = $(this),
				$item = $this.parents('.item'),
				$btn_edit = $('.btn_edit', $item);
			$btn_edit.removeClass('open');
			$('.form_container', $item).slideToggle(300, function () {
				$('span', $btn_edit).toggle();
			});
		});

		$('[id^=form_edit]').live('submit', function (event) {
			event.preventDefault();
			var $form = $(this),
				$item = $form.parents('.item'),
				$itemId = $item.attr('id').split('item-')[1],
				$term = $form.find('textarea[name="term"]').val(),
				$title = $form.find('input[name="title"]').val(),
				url = $form.attr('action');

			$form.append('<div class="saved" style="opacity:0"><p>Saving...</p></div>');
			$('.saved', $form).animate({ opacity: 1 }, 300);

			$.ajax({
				type: "POST",
				url: url,
				data: { id: $itemId, term: $term, title: $title },
				success: function (data) {
					$('.saved', $form).animate({ opacity: 0 }, 300, function () {
						//$('.arc_res p', $item).html('Saved');
						$item.wrap('<div class="holder" />');
						var itemWrapper = $item.parents('.holder'),
								$updatedData = $(data).filter('div');
						$item.animate({ opacity: 0 }, 300);
						$(itemWrapper).slideUp(500, function () {
							$item.replaceWith($updatedData);
							$('.item', this).animate({ opacity: 0 }, 0);
							$(itemWrapper).delay(300).slideDown(500, function () {
								if ($('.item', this).find(Input.inputClass).val().length > 0) {
									$('.item', this).find(Input.inputClass).show();
								};
								$('.item', this).animate({ opacity: 1 }, 300).unwrap();
							});
						});
					});
				}
			});
		});
	},

	item_remove: function ($item) {
		$item.animate({ opacity: 0 }, 300, function () {
			$item.slideUp(300, function () {
				$item.remove();
			});
		});
	}

};

var Interact = {

	itemClass: '.item',
	expandClass: '.expand',
	controlsClass: '.controls_container',
	toggleClass: 'open',

	expand: function () {
		$(Interact.itemClass).live('click', function (event) {
			if (event.target.nodeName == 'A' || event.target.nodeName == 'INPUT' || event.target.nodeName == 'TEXTAREA') return;
			var $this = $(this)
			$(Interact.expandClass, $this).slideToggle(250);
			$this.toggleClass(Interact.toggleClass);
			if ($('iframe', $this).length && $('iframe', $this).attr('src') === undefined && $this.hasClass(Interact.toggleClass)) {
				$('iframe', $this).attr('src', $(Interact.expandClass, $this).attr('data-src'));
			};
			event.preventDefault();
		});
	},

	hover: function () {
		$(Interact.controlsClass).animate({ opacity: 0 }, 0);
		$(Interact.itemClass).live('mouseover', function () {
			$(Interact.controlsClass, this).css('visibility', 'visible').stop().animate({ opacity: 1 }, 150);
		});
		$(Interact.itemClass).live('mouseout', function () {
			if (!$(this).hasClass(Interact.toggleClass)) {
				$(Interact.controlsClass, this).stop().animate({ opacity: 0 }, 150);
			};
		});
	}

};

var Search = {

	input: '#search_filter',
	wrapper: '.item_container',
	item: '.item',
	match: 'match',

	init: function () {
		// Extend jQuery by adding case insensitive :contains selector
		jQuery.extend(jQuery.expr[":"], { "contains-any": function(elem, i, match, array) { return (elem.textContent || elem.innerText || $(elem).text() || "").toLowerCase().indexOf((match[3] || "").toLowerCase()) >= 0; } });

		Search.filter();
		Search.toggle();

	},

	toggle: function() {

		$('.search_link').on('click', function (e) {
			$(this).toggleClass('open');
			$('.search_input').toggle().focus().attr('value', '');
			$('.item').show();
			e.preventDefault();
		});

	},

	filter: function () {

		$(Search.input).on('keyup', function () {
			var val = $(this).val();
			// filter items
			if (val.length) {
				$(Search.item).hide().removeClass(Search.match);
				$('h2:contains-any("' + val + '"), p:contains-any("' + val + '"), .inner:contains-any("' + val + '")').parents(Search.item).show().addClass(Search.match);
				$(Search.item + '[class*=' + val + ']').show().addClass(Search.match);
			} else {
				$(Search.item).show().addClass(Search.match);
			};
			// toggle feedback
			if (!$('.' + Search.match).length && !$('.feedback').length) {
				$(Search.wrapper).prepend('<p class="feedback">No results</p>');
			};
			if ($('.' + Search.match).length && $('.feedback').length) {
				$('.feedback').remove();
			};
		});

	}

};

var Account = {
    container: '.profile',
    avatar: '.avatar',
    menu: '.account',

    dropdown: function() {

        $(Account.avatar).on('click', function(e) {
            var $this = $(this);
            $this.next().toggle().toggleClass('open');
            $this.toggleClass('selected');
            e.stopPropagation();
            e.preventDefault();
        });
        
        $('body').not(Account.menu).on('click', function (e) {
            if ($(Account.avatar).hasClass('selected') && !$(e.target).hasClass(Account.menu.slice(1)) && e.target.nodeName != 'A') {
                $(Account.avatar).removeClass('selected').next().hide().removeClass('open');
                e.stopPropagation();
                e.preventDefault();
            };
        });
    }

};

var Input = {

	inputClass: '.input_title',
	resetClass: '.reset',
	parentClass: '.title',

	inputKeypress: function () {

		$(Input.inputClass).live({
			focus: function () {
				var $this = $(this);
				Input.testValue($this);
			},
			blur: function () {
				var $this = $(this);
				Input.testValue($this);
			},
			keypress: function () {
				var $this = $(this);
				Input.testValue($this);
			},
			keyup: function () {
				var $this = $(this);
				Input.testValue($this);
			}
		});

	},

	testValue: function ($this) {
		if ($this.val().length > 0) {
			Input.showButton($this);
		} else {
			Input.hideButton($this);
		};
	},

	showButton: function ($this) {
		$(Input.resetClass, $this.parent()).show();
	},

	hideButton: function ($this) {
		$(Input.resetClass, $this.parent()).hide();
	},

	inputClear: function () {
		$(Input.resetClass).live('click', function () {
			$this = $(this).parents(Input.parentClass);
			$(Input.inputClass, $this).attr('value', '');
			$(Input.inputClass, $this).focus();
			Input.hideButton($this);
			return false;
		});
	}


};

var Utils = {

	externalLink: function () {
		$('.external').live('click', function (e) {
			var link = $(this).attr('href');
			window.open(link);
			e.preventDefault();
		});
	}

};


var Debug = {

	log: function (str) {
		/// <summary>Wrapper for console.log to enable intellisense</summary>
		/// <param name="str" type="String">Value to log in the console</param>
		if (AppSettings.DEBUGMODE) {
			try { console.log(str); }
			catch (e) { }
		}
	}

};

$(document).ready(Main.run());