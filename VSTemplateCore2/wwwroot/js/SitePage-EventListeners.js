$(function() {
	// When 'Show in Menu' is unchecked, adjust the form accordingly
	$('#ShowInMenu').on('click', function() {
		// If the 'This is a Menu Section' option is selected, then don't hide the Menu Title field because it is still needed
		if ($('#ParentMenuId').children('option:selected').text() != 'This is a Menu Section') {
			$('[data-field="MenuTitle"]').toggle();
		}
	});

	// When a Parent Menu is selected from the list, modify the form accordingly
	$('#ParentMenuId').on('change', function() {
		if ($(this).children('option:selected').text() == 'This is a Menu Section') {
			// Make sure the 'Show in Menu' checkbox is checked and make sure the Menu Title field is showing
			$('#ShowInMenu').prop('checked', true);
			$('[data-field="MenuTitle"]').show();
			// Make sure that Admins cannot delete the page
			$('#AllowDelete').prop('checked', false).prop('disabled', true);
			$('[data-menu-section-field="false"]').hide();
		} else {
			if ($(this).children('option:selected').text() == 'This is a Root Page') {
				$('#AllowDelete').prop('checked', false).prop('disabled', true);
			} else {
				$('#AllowDelete').prop('disabled', false).prop('checked', true);
			}
			$('[data-menu-section-field="false"]').show();
		}
	});

	// When a Page Title is entered, copy the text to the Menu Title and Page Name fields as they will likely be the same text
	$('#Title').on('change', function() {
		$('#MenuTitle').val($(this).val());
		$('#Name').val($(this).val());
	});
});
