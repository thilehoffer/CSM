/// <reference path="../jquery-1.7.1.min.js" />
$(document).ready(function () {

    $('#cancelDisabilityCategory').click(function (event) {
        event.preventDefault();
        window.location = '/DisabilityCategory';
    });
    

    $('#saveDisabilityCategory').click(function (event) {
        event.preventDefault();
        
        var postData = $('#editDisabilityCategoryForm').serialize();
        var url = '/DisabilityCategory/Edit';
        
        $.post(url, postData,
        function (data) {

            var jsonAjaxResult = new CaseloadManager.JsonAjaxResult();
            jsonAjaxResult.init(data);
            
            if (jsonAjaxResult.success) {
                window.location = '/DisabilityCategory';
            }
            else {
                $('#postSaveDisabilityCategory').html(jsonAjaxResult.errorListToHtml());
            }
        });

       
    });
    

});