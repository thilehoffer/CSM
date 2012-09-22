/// <reference path="../jquery-1.7.1.min.js" />
$(document).ready(function () {

    $('#cancelCreateDisabilityCategory').click(function (event) {
        event.preventDefault();
        window.location = '/DisabilityCategory';
    });


    $('#createDisabilityCategory').click(function (event) {
        event.preventDefault();

        var postData = $('#createtDisabilityCategoryForm').serialize();
        var url = '/DisabilityCategory/Create';

        $.post(url, postData,
        function (data) {

            var jsonAjaxResult = new CaseloadManager.JsonAjaxResult();
            jsonAjaxResult.init(data);

            if (jsonAjaxResult.success) {
                window.location = '/DisabilityCategory';
            }
            else {
                $('#postCreateDisabilityCategory').html(jsonAjaxResult.errorListToHtml());
            }
        });


    });


});