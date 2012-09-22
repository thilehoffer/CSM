

$(document).ready(function () {
    var sharableDataSource = new kendo.data.DataSource(
                {
                    type: "json",
                    transport: { read: "/FormData/GetUpcomingEvents" },
                    schema: {
                        model: {
                            fields: {
                                OccursIn : { type: "string"},
                                ScheduledOnString: { type: "string" },
                                EventType: { type: "string" },
                                StudentName: { type: "string" },
                                Complete: { type: "string" }
                            }
                        }
                    },
                    serverPaging: false,
                    serverFiltering: false,
                    serverSorting: false
                }
         );


    $("#grid").kendoGrid({
        dataSource: sharableDataSource,
        groupable: false,
        scrollable: false,
        sortable: true,
        pageable: false,
        filterable: true,
        columns: [
        {
            field: "OccursIn",
            title: "Occurs In"
        },
         {
             field: "ScheduledOnString",
             title: "Scheduled For"
         },
        {
            field: "EventType",
            title: "Event"
        },
        {
            field: "StudentName",
            title: "Student Name",
            template: '<a  href="/Student/Details/#=StudentId#">#=StudentName#</a>'
        },
        {
            field: "Complete",
            title: "Complete"
        } 
        ]
    });




    $('#searchInput').bind('keyup', function () {

        sharableDataSource.filter([{
            filters:
            [
        { field: "StudentName", operator: "contains", value: this.value }]
        }]);

    });
});
