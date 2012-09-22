
$(document).ready(function () {
    //Transform our items to use kendo widgets 
    $("#DateOfContact").kendoDatePicker();
    $("#TimeOfContact").kendoTimePicker();
    $("#StudentParentId").kendoDropDownList();

    $('#cancelButton').click(function (event) {
        event.preventDefault();
        LoadParentContacts();
    });


    $("#saveContactRecord").click(function (event) {
        event.preventDefault();

        var postData = $('#EditStudentParentContactForm').serialize();
        var url = '/StudentParentContact/Edit';

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
                    LoadParentContacts();
                }
                else {
                    $('#postSaveDD').html(jsonAjaxResult.errorListToHtml());
                }
            });
    });

});