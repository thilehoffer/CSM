$(document).ready(function () {
    //Transform our items to use kendo widgets 
    $("#ScheduledDate").kendoDatePicker();
    $("#ScheduledDateTime").kendoTimePicker();

    $("#DateOfMeeting").kendoDatePicker();
    $("#DateOfMeetingTime").kendoTimePicker();



    $("#saveIEP").click(function (event) {

        event.preventDefault();

        var postData = $('#CreateIEPForm').serialize();
        var url = '/StudentIEP/Create';

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
                    $('#postCreateIEPDD').html(jsonAjaxResult.errorListToHtml());
                }
            });
    });
});