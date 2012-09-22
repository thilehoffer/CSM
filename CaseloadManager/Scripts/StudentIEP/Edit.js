$(document).ready(function () {
    //Transform our items to use kendo widgets 
    $("#ScheduledDate").kendoDatePicker();
    $("#ScheduledDateTime").kendoTimePicker();

    $("#DateOfMeeting").kendoDatePicker();
    $("#DateOfMeetingTime").kendoTimePicker();

    var validatable = $("#EditIEPForm").kendoValidator().data("kendoValidator");

    $("#saveIEP").click(function (event) {

        event.preventDefault();
        if (!validatable.validate()) {
            return false;
        }

        var postData = $('#EditIEPForm').serialize();
        var url = '/StudentIEP/Edit';

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
                    LoadIeps();
                }
                else {
                    $('#postCreateIEPDDpostEditIEPDD').html(jsonAjaxResult.errorListToHtml());
                }
            });
    });

    $("#cancelIEP").click(function (event) {
        event.preventDefault();
        LoadIeps();
    });
});