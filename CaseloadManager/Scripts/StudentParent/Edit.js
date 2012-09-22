$(document).ready(function () {
    $("#PreferredContactMethod").kendoDropDownList();


    $("#cancelEditParent").click(function (event) {
        event.preventDefault()
        LoadParents();
    });

    $("#editParent").click(function (event) {

        event.preventDefault();
        var postData = $('#EditParentForm').serialize();
        var url = '/StudentParent/Edit';
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
                    $('#postEditParentSaveDD').html(jsonAjaxResult.errorListToHtml());
                }
            });

    });
});