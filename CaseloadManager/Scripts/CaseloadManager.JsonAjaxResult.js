CaseloadManager.JsonAjaxResult = function () {

    var errorList = new Array();
    var success = true;
    var noAccess = false;
    var notFound = false;

    this.init = function(data) {

        this.errorList = data.errorList;
        this.success = data.success;
        this.noAccess = data.noAccess;
        this.notFound = data.notFound;

    };

    this.errorListToHtml = function() {
        var errorListHtml = '';
        for (var i = 0; i < this.errorList.length; i++) {
            errorListHtml += '<span class="error">' + this.errorList[i] + '</span>';
            if (i < this.errorList.length - 1) {
                errorListHtml += '<br/>';
            }


        }
        return errorListHtml;
    }; 
   
}
 
   
       

   
 