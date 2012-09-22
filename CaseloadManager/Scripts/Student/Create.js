/// <reference path="../jquery-1.7.1.js" />
/// <reference path="../CaseloadManager.JsonAjaxResult.js" />
/// <reference path="../../Content/Kendu/js/kendo.all.min.js" />
/// <reference path="../../Content/Kendu/js/kendo.web.min.js" />




$(document).ready(function () {
    //Transform our items to use kendo widgets 
    $("#DateOfBirth").kendoDatePicker();
    $("#DateOfEntry").kendoDatePicker();
    $("#ExpectedGraduationYear").kendoDropDownList();
    $("#PrimaryDisabilityId").kendoDropDownList();
    $("#SecondaryDisabilityId").kendoDropDownList();
    
    
   


    $('#save').click(function (event) {
        event.preventDefault();
        var postData = $('#createStudentForm').serialize();
        var url = '/Student/Create';
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
                    window.location = '/Student/Index';
                }
                else {
                    $('#postSubmitDD').html(jsonAjaxResult.errorListToHtml());
                }
            });

    });

});