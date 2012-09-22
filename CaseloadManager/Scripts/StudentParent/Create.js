

$(document).ready(function () {
    $("#PreferredContactMethod").kendoDropDownList();

    $("#cancelSaveParent").click(function (event) {
        event.preventDefault();
        LoadParents();
    });

    $("#saveParent").click(function (event) {

        event.preventDefault();
        var postData = $('#CreateParentForm').serialize();
        var url = '/StudentParent/Create';

        $.post(url, postData,
            function (data) {
                var jsonAjaxResult = new CaseloadManager.JsonAjaxResult();
                jsonAjaxResult.init(data);

                if (jsonAjaxResult.noAccess) {
                    window.location = '/NoAccess';
                    return;
                }
                if (jsonAjaxResult.notFound) {
                    window.location = '/NotFound';
                    return;
                }
                if (jsonAjaxResult.success) {

                    LoadParents();
                }
                else {
                    $('#postSaveDD').html(jsonAjaxResult.errorListToHtml());
                }
            });

    });
});

