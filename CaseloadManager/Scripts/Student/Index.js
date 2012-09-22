

function UpdateCurrent(id, studentId) {
     
    var checked = $('#' + id).is(':checked'); 
    var loadingImg = $('#img' + studentId);
    loadingImg.css('visibility', 'visible');
    var postUrl = 'Student/SetCurrent';
    var postData = { id: studentId, current: checked };
    $.post(postUrl, postData,
        function (data) {
            var jsonAjaxResult = new CaseloadManager.JsonAjaxResult();
            jsonAjaxResult.init(data);

            if (jsonAjaxResult.noAccess) {
                window.location = "/NoAccess";
                return;
            } else if (jsonAjaxResult.notFound) {
                window.location = "/NotFound";
                return;
            } else if (jsonAjaxResult.success) {
                loadingImg.css('visibility', 'hidden');
            }
            else {
                alert('An error occured');
            }
        }
    );
}


$(document).ready(function () {
    var _includePrevious = false;
    var sharableDataSource = new kendo.data.DataSource(
              {

                  type: "json",
                  transport: {

                      read: {
                          url: "/FormData/GetStudentList",
                          data: { includePrevious: function () { return _includePrevious; } }

                      }
                  }
                  ,
                  schema: {
                      model: {
                          fields: {
                              FirstName: { type: "string" },
                              LastName: { type: "string" },
                              ExpectedGraduationYear: { type: "number" },
                              PrimaryDisability: { type: "string" },
                              LocalEducationAgency: { type: "string" },
                              StudentId: { type: "string" },
                              CurrentStudent: { type: "boolean" },
                              CurrentCheckBox: { type: "string" }
                          }
                      }
                  },
                  serverPaging: false,
                  serverFiltering: false,
                  serverSorting: false
              }
       );

    var selectCurrentOnly = $('#selectCurrentOnly').kendoDropDownList();
    var optionsDiv = $('#optionsDiv');
    var searchOptionsSpan = $('#searchOption');
    var cancelOptions = $('#cancelOptions');
    var applyOptions = $('#applyOptions');
    optionsDiv.hide();
    var studentGrid = $("#grid").kendoGrid({
        dataSource: sharableDataSource,
        groupable: false,
        scrollable: false,
        sortable: true,
        pageable: false,
        filterable: true,
        dataBound: function (e) {
           
        },
        columns: [
        {
            field: "FirstName",
            title: "First Name"
        },
        {
            field: "LastName",
            title: "Last Name"
        },
        {
            field: "ExpectedGraduationYear",
            title: "Grad. Year"
        },
        {
            field: "PrimaryDisability",
            title: "Primary Disability"
        },
        {
            field: "LocalEducationAgency",
            title: "LEA"
        },
        {
            title: "Active",
            field: "CurrentStudent",
            template: '<div id="CurrentRowDiv"><input type="checkbox"  #=CurrentCheckBox# onchange="UpdateCurrent(\'cb#=StudentId#\', #=StudentId#)" id="cb#=StudentId#" /> <img id="img#=StudentId#"src="../../Content/Kendu/styles/Default/loading-image.gif"  /></div> <div class="clear-empty"></div>'
        },
        {
            title: "Details",
            field: "StudentId",
            sortable: false,
            filterable: false,
            template: '<a class="graySmall" href="/Student/Details/#=StudentId#">Details</a>'
        }

        ]
    });

    searchOptionsSpan.click(function () {
        if (optionsDiv.is(':visible')) {
            optionsDiv.hide('fast');
        } else {
            optionsDiv.show('fast');
        }
    });

    cancelOptions.click(function (event) {
        event.preventDefault();
        optionsDiv.hide('fast');
    });

    applyOptions.click(function (event) {
        event.preventDefault();
        _includePrevious = selectCurrentOnly.val() === 'No' ? false : true;
        sharableDataSource.read();
        optionsDiv.hide('fast');
    });





    $('#searchInput').bind('keyup', function () {

        sharableDataSource.filter([{
            logic: "or", filters:
            [
        { field: "FirstName", operator: "contains", value: this.value },
        { field: "LastName", operator: "contains", value: this.value }]
        }]);
    });




});
