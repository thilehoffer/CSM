$(document).ready(function () {
    //Transform our items to use kendo widgets 
    $("#ScheduledDate").kendoDatePicker();
    $("#DateCompleted").kendoDatePicker();

    var validatable = $("#EditEvaluationForm").kendoValidator().data("kendoValidator");

    $("#saveEvaluation").click(function (event) {

        event.preventDefault();
        if (!validatable.validate()) {
            return false;
        }

        var postData = $('#EditEvaluationForm').serialize();
        var url = '/StudentEvaluation/Edit';

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
                    LoadEvaluations();
                }
                else {
                    $('#postEditEvaluationDD').html(jsonAjaxResult.errorListToHtml());
                }
            });
        return true;
    });

    $("#cancelEvaluation").click(function (event) {
        event.preventDefault();
        LoadEvaluations();
    });

});
