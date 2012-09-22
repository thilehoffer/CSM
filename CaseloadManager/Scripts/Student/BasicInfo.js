var basicInfo = new Object();
basicInfo.postLoad = function () {
    //Transform our items to use kendo widgets  
    $("#DateOfBirth").kendoDatePicker();
    $("#DateOfEntry").kendoDatePicker();
    $("#ExpectedGraduationYear").kendoDropDownList();
    $("#PrimaryDisabilityId").kendoDropDownList();
    $("#SecondaryDisabilityId").kendoDropDownList();
    $("#CurrentStudent").kendoDropDownList();
    $("#saveBasicInfo").click(function (event) {

        event.preventDefault();
       

        var postData = $('#basicInfoForm').serialize();
        var url = ("/Student/BasicInfoPost");
        $.post(url, postData,
             function (data) {

                 var jsonAjaxResult = new CaseloadManager.JsonAjaxResult();
                 jsonAjaxResult.init(data);

                 if (jsonAjaxResult.noAccess) {
                     window.location = "/NoAccess";
                     return;
                 }

                 if (jsonAjaxResult.notFound) {
                     window.location = "/NotFound";
                     return;
                 }


                 if (jsonAjaxResult.success) {
                     $('#postSaveBasicDD').html("<span class='success'>Save Successful</span>");
                 }
                 else {
                     $('#postSaveBasicDD').html(jsonAjaxResult.errorListToHtml());
                 }
             }
            );
    });
};