CaseloadManager.Helpers = {};

String.isNullOrEmpty = function (str) {
    return (str === '' || str === 'null' || str === null || str === undefined);
};

CaseloadManager.Helpers.GetHostName = function () {

    var anchor = $("<a />").attr('href', window.location);
    //changed this to use the anchor because the jquery was returning undefined.
    var a = anchor[0];

    if (String.isNullOrEmpty(a.port)) {
        return a.protocol + "//" + a.hostname;
    }
    else {
        return a.protocol + "//" + a.hostname + ":" + a.port;
    }
};
CaseloadManager.Helpers.DeleteDocument = function (id) { 

    var url = CaseloadManager.Helpers.GetHostName() + "/Document/Delete/" + id;
    var postData = { StudentIEDId: id };
    $.post(url, postData,
                function (data) {
                    var jsonAjaxResult = new CaseloadManager.JsonAjaxResult();
                    jsonAjaxResult.init(data);
                    if (jsonAjaxResult.success) {
                        return true;
                    }
                    else {
                        return false;
                    }
                });


};
$(function () {
    // check placeholder browser support
    if (!Modernizr.input.placeholder) {

        // set placeholder values
        $(this).find('[placeholder]').each(function () {
            if ($(this).val() == '') // if field is empty
            {
                $(this).val($(this).attr('placeholder'));
            }
        });

        // focus and blur of placeholders
        $('[placeholder]').focus(function () {
            if ($(this).val() == $(this).attr('placeholder')) {
                $(this).val('');
                $(this).removeClass('placeholder');
            }
        }).blur(function () {
            if ($(this).val() == '' || $(this).val() == $(this).attr('placeholder')) {
                $(this).val($(this).attr('placeholder'));
                $(this).addClass('placeholder');
            }
        });

        // remove placeholders on submit
        $('[placeholder]').closest('form').submit(function () {
            $(this).find('[placeholder]').each(function () {
                if ($(this).val() == $(this).attr('placeholder')) {
                    $(this).val('');
                }
            });
        });

    }
});

var dateRegExp = /^\/Date\((.*?)\)\/$/;
function toDate(value) {
    var date = dateRegExp.exec(value);
    return new Date(parseInt(date[1]));
}