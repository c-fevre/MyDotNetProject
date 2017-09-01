//////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////
// NOTIFICATION
//////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////
function popNotification(type, delay, message, onClose) {
    $.notify({
        // options
        message: message,
        placement: {
            from: "bottom"
        }
    },
	{
	    // settings
	    type: type,
	    delay: delay,
	    animate: {
	        enter: "animated fadeInRight",
	        exit: "animated fadeOutRight"
	    },
	    offset: {
	        y: 60,
	        x: 10
	    },
	    newest_on_top: true,
	    onClose: onClose

	});
}


$(function () {
    //display regular notifications from the model
    $("#notificationHiddenDiv div").each(function () {
        var message = $(this).data("title");
        var delay = $(this).data("delay");
        var type = $(this).data("type");

        popNotification(type, delay, message);
    });

    //display user notifications from the model
    $("#userNotificationHiddenDiv div").each(function () {
        var message = $(this).data("title");
        var delay = $(this).data("delay");
        var type = $(this).data("type");
        var id = $(this).data("id");
        var url = $(this).data("url-hasbeenread");

        var onClose = function () {
            $.ajax({
                type: "GET",
                url: url,
                success: function () {
                },
                error: function (error) {
                    console.log(error);
                }
            });
        };

        popNotification(type, delay, message, onClose);
    });

});

//////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////
// MODAL
//////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////
function alertModal(message) {
    var modalBody = $("#alert-modal .modal-body");
    modalBody.empty();

    // Split the message on new lines and put each line in a DIV
    var lines = message.split(/\r?\n/);
    $.each(lines,
		function (i, e) {
		    modalBody.append($("<div>").text(e));
		});

    var deferred = $.Deferred();

    $("#alert-modal").modal();
    $("#alert-modal")
		.on("hide.bs.modal",
			function () {
			    deferred.reject();
			});
    return deferred.promise();
}

//////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////
// OTHER
//////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////

/*
    String.format
    JavaScript version of the C# String.Format.

    Two versions available:
    Indexed by number:      String.format("bla {2} bla {0} bla {1}", arg0, arg1, arg2)
    With named parameters:  String.format("bla {firstArg} bla {thirdArg} bla {secondArg}", { firstArg: 42, secondArg: 44, thirdArg: 43 })

    Format strings are not (yet?) supported.
*/
String.format = function () {
    var args = arguments;
    var formatString = args[0];

    if (args.length == 2 &&
		$.isPlainObject(args[1])) {
        formatString = formatString.replace(
			/\{([_a-zA-Z][_a-zA-Z0-9]*)\}/g,
            function (m, p1, offset, string) {
                return args[1][p1];
            });
    }
    else {
        formatString = formatString.replace(
			/\{(\d+)\}/g,
            function (m, p1, offset, string) {
                return args[parseInt(p1) + 1];
            });
    }

    return formatString;
};

//-------------------------------------------------------------------------------------------------

if (!MyContractsGenerator) var MyContractsGenerator = {};

//-------------------------------------------------------------------------------------------------
// changesManager
// Manages the confirmation message when leaving the page.

function changesManagerCtor() {
    var installed = false;
    var leaveMessage = "";
    var formSelector;
    var initialFormData = "";
    var checkChanges = true;
    var buttonCancel = null;
    var buttonSave = null;
    var inputsToExclude = [];

    function ensureInstalled() {
        if (installed)
            return false;

        window.onbeforeunload = function () {
            var currentFormData = $(formSelector).html();

            if (checkChanges && currentFormData !== initialFormData) {
                return leaveMessage;
            }
        };
        installed = true;
    }

    // Bypass changes (submit or cancel button)
    function setPageChecking(state) {
        if (!installed)
            throw new Error("init must be called first");
        checkChanges = state;
    }

    // Sets a confirmation on exit with the message supplied
    // The buttons are updated when setPageChanged/setPageSaved are called (disabled or not).
    // The save button automatically sets the page as saved.
    function init(message, buttonSaveSelector, buttonCancelSelector, checkFormSelector, inputsToExcludeSelectors) {
        ensureInstalled();
        leaveMessage = message;
        if (checkFormSelector) {
            formSelector = checkFormSelector;
            initialFormData = $(formSelector).html();
        }
        if (buttonSaveSelector) buttonSave = $(buttonSaveSelector);
        if (buttonCancelSelector) buttonCancel = $(buttonCancelSelector);
        //updateButtons();
        if (buttonSave) buttonSave.on("click", function () { setPageChecking(false) });
        if (buttonCancel) buttonCancel.on("click", function () { setPageChecking(false) });
        if (inputsToExcludeSelectors) {
            /*inputsToExcludeSelectors.forEach(function (item) {
                inputsToExclude.push($(item));
                $(item).on("click", function () { setPageChecking(false) });
            });*/
            ////IE 8 Patch :
            for (var i = 0; i < inputsToExcludeSelectors.length; i++) {
                inputsToExclude.push($(inputsToExcludeSelectors[i]));
                $(inputsToExcludeSelectors[i]).on("click", function () { setPageChecking(false) });
            }
        }
    }

    return {
        init: init,
        setPageChecking: setPageChecking
    };
}

MyContractsGenerator.changesManager = changesManagerCtor();



jQuery.fn.isVisible = function () {
    // Current distance from the top of the page
    var windowScrollTopView = jQuery(window).scrollTop();

    // Current distance from the top of the page, plus the height of the window
    var windowBottomView = windowScrollTopView + jQuery(window).height() + 65;// 400 = threshold

    // Element distance from top
    var elemTop = jQuery(this).offset().top;

    // Element distance from top, plus the height of the element
    var elemBottom = elemTop + jQuery(this).height();

    return ((elemBottom <= windowBottomView) && (elemTop >= windowScrollTopView));
}